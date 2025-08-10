using Microsoft.Extensions.Configuration;
using StockQuoteAlert.Enums;
using StockQuoteAlert.Services;
using StockQuoteAlert.Settings;
using System.Globalization;

public class Program
{
    public static async Task Main(string[] args)
    {
        // 1. Validar os parâmetros da linha de comando
        if (args.Length < 3)
        {
            Console.WriteLine("Uso: StockQuoteAlert.exe <ativo> <preco_venda> <preco_compra>");
            return;
        }

        string ticker = args[0].ToUpper();
        decimal sellPrice = decimal.Parse(args[1], CultureInfo.InvariantCulture);
        decimal buyPrice = decimal.Parse(args[2], CultureInfo.InvariantCulture);

        // 2. Carregar configurações
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var smtpSettings = config.GetSection("SmtpSettings").Get<SmtpSettings>();
        var alertSettings = config.GetSection("AlertSettings").Get<AlertSettings>();
        var apiSettings = config.GetSection("ApiSettings").Get<ApiSettings>();

        // 3. Inicializar os serviços
        var emailService = new EmailService(smtpSettings);
        var stockService = new StockService(apiSettings.BaseUrl, apiSettings.ApiKey);
        var cultureInfo = new CultureInfo("pt-BR");

        Console.WriteLine($"Monitorando o ativo: {ticker}");
        Console.WriteLine($"Preço de venda (Alerta): {sellPrice.ToString("C", cultureInfo)}");
        Console.WriteLine($"Preço de compra (Alerta): {buyPrice.ToString("C", cultureInfo)}");
        Console.WriteLine("---------------------------------------------");

        // Variável de controle para evitar e-mails repetidos
        var lastAlertState = AlertState.Normal;

        // 4. Loop de monitoramento contínuo
        while (true)
        {
            try
            {
                decimal currentPrice = await stockService.GetCurrentPrice(ticker);
                Console.WriteLine($"[INFO] Cotação atual de {ticker}: {currentPrice.ToString("C", cultureInfo)} em {DateTime.Now:T}");

                if (currentPrice > sellPrice)
                {
                    if (lastAlertState != AlertState.Venda)
                    {
                        string subject = $"ALERTA DE VENDA: {ticker} em {currentPrice.ToString("C", cultureInfo)}";
                        string body = $"A cotação de {ticker} atingiu {currentPrice.ToString("C", cultureInfo)}, que é maior que o preço de referência de venda de {sellPrice.ToString("C", cultureInfo)}.";
                        await emailService.SendAlertEmail(alertSettings.ToEmail, subject, body);
                        lastAlertState = AlertState.Venda;
                    }
                }
                else if (currentPrice < buyPrice)
                {
                    if (lastAlertState != AlertState.Compra)
                    {
                        string subject = $"ALERTA DE COMPRA: {ticker} em {currentPrice.ToString("C", cultureInfo)}";
                        string body = $"A cotação de {ticker} atingiu {currentPrice.ToString("C", cultureInfo)}, que é menor que o preço de referência de compra de {buyPrice.ToString("C", cultureInfo)}.";
                        await emailService.SendAlertEmail(alertSettings.ToEmail, subject, body);
                        lastAlertState = AlertState.Compra;
                    }
                }
                else
                {
                      lastAlertState = AlertState.Normal;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"\n[ERRO] Ocorreu um erro no monitoramento: {ex.Message}");
                Console.ResetColor();
            }

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}