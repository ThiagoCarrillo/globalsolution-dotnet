using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendNewPatientNotification(string doctorEmail, string patientName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sistema de Saúde", _config["EmailSettings:Username"]));
        message.To.Add(new MailboxAddress("Médico", doctorEmail));
        message.Subject = "Novo Paciente Cadastrado";

        message.Body = new TextPart("plain")
        {
            Text = $"Olá Dr(a), \n\nUm novo paciente foi cadastrado: {patientName}."
        };

        using var client = new SmtpClient();
        client.Connect(
            _config["EmailSettings:SmtpServer"],
            int.Parse(_config["EmailSettings:Port"]),
            MailKit.Security.SecureSocketOptions.StartTls);

        client.Authenticate(
            _config["EmailSettings:Username"],
            _config["EmailSettings:Password"]);

        client.Send(message);
        client.Disconnect(true);
    }
}