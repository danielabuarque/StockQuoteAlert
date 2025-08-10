using StockQuoteAlert.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _settings;
        private readonly string _fromAddress;

        public EmailService(SmtpSettings settings)
        {
            _settings = settings;
            _fromAddress = settings.Username;
        }

        public async Task SendAlertEmail(string toAddress, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_settings.Server, _settings.Port))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

                    var mailMessage = new MailMessage(_fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = false // Para e-mail em texto puro
                    };

                    await client.SendMailAsync(mailMessage);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[ALERTA ENVIADO] E-mail disparado para {toAddress}.");
                    Console.ResetColor();
                }
            }
            catch (SmtpException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERRO SMTP] Falha ao enviar e-mail. Verifique as configurações de SMTP: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERRO GERAL] Ocorreu um erro ao tentar enviar o e-mail: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
