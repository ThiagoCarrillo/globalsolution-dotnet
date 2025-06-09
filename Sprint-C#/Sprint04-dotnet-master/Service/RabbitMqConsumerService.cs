using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sessions_app.Models;
using System.Text;
using System.Text.Json;


namespace Sessions_app.Service
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly EmailService _emailService;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqConsumerService(IConfiguration config, EmailService emailService, IServiceProvider serviceProvider)
        {
            _config = config;
            _emailService = emailService;
            InitializeRabbitMq();
            _serviceProvider = serviceProvider;
        }

        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:HostName"],
                Port = int.Parse(_config["RabbitMQ:Port"]),
                UserName = _config["RabbitMQ:UserName"],
                Password = _config["RabbitMQ:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declara a exchange (se não existir)
            _channel.ExchangeDeclare(
                exchange: _config["RabbitMQ:Exchange"],
                type: ExchangeType.Direct,
                durable: true,  
                autoDelete: false);

            // Declara a fila com propriedades específicas
            _channel.QueueDeclare(
                queue: "notificacoes-pacientes", // Nome explícito
                durable: true,     // Fila persiste após reinicialização
                exclusive: false,  // Não é exclusiva para esta conexão
                autoDelete: false, // Não é apagada quando desconectada
                arguments: null);

            // Liga a fila à exchange com a routing key
            _channel.QueueBind(
                queue: "notificacoes-pacientes",
                exchange: _config["RabbitMQ:Exchange"],
                routingKey: _config["RabbitMQ:RoutingKey"]);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Desserializa para o DTO correto
                    var notification = JsonSerializer.Deserialize<NotificationDto>(message);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

                        // Envia email para o médico específico da mensagem
                        emailService.SendNewPatientNotification(
                            notification.MedicoEmail,
                            notification.Paciente.Nome);
                    }

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _channel.BasicNack(ea.DeliveryTag, false, true); // Recoloca na fila
                }
            };

            _channel.BasicConsume(
                queue: _config["RabbitMQ:QueueName"],
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
