using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockQuoteAlert.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockQuoteAlert.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class StockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public StockService(string baseUrl, string apiKey)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _apiKey= apiKey;
        }

        public async Task<decimal> GetCurrentPrice(string assetSymbol)
        {
            //Creating the URL for the API request
            string url = $"{_baseUrl}/{assetSymbol}?token={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);

            try
            {
                var apiResponse = JsonConvert.DeserializeObject<StockApiResponseDto>(response);

                if(apiResponse?.Results != null && apiResponse.Results.Any())
                {
                    return apiResponse.Results[0].RegularMarketPrice;
                }
            }
            catch (JsonSerializationException ex)
            {
                Console.WriteLine($"Erro ao desserializar a resposta da API: {ex.Message}");
            }

            throw new InvalidOperationException("Não foi possível extrair a cotação do ativo da resposta da API.");
        }
    }
}
