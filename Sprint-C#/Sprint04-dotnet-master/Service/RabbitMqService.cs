using RabbitMQ.Client;
using Sessions_app.Models;
using System.Text;
using System.Text.Json;


namespace Sessions_app.Service
{
    public class RabbitMqService 
    {
        private readonly IConfiguration _config;

        public RabbitMqService(IConfiguration config)
        {
            _config = config;
        }

        public void PublishNewPatient(Paciente paciente, List<string> emailsMedicos)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMQ:HostName"],
                Port = int.Parse(_config["RabbitMQ:Port"]),
                UserName = _config["RabbitMQ:UserName"],
                Password = _config["RabbitMQ:Password"]
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Declara a exchange uma única vez
            channel.ExchangeDeclare(
                exchange: _config["RabbitMQ:Exchange"],
                type: ExchangeType.Direct,
                durable: true);

            // Cria um objeto de notificação completo
            foreach (var emailMedico in emailsMedicos)
            {
                var notification = new
                {
                    Paciente = new
                    {
                        paciente.Nome,
                        paciente.DataNascimento,
                        paciente.Telefone
                    },
                    MedicoEmail = emailMedico,
                    Timestamp = DateTime.UtcNow
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(notification));

                channel.BasicPublish(
                    exchange: _config["RabbitMQ:Exchange"],
                    routingKey: _config["RabbitMQ:RoutingKey"],
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
