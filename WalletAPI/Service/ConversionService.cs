using System.Text.Json;
using System.Net.Http;
using WalletAPI.Models.DTO;
using WalletAPI.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;

namespace WalletAPI.Service
{
    public class ConversionService: IConversionService
    {
        private readonly HttpClient _httpClient;
        private const string RapidApiKey = "f58bacf1aemshf6f109d7dbbc52ap14030ejsn99d77aa12db7";

        public ConversionService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", RapidApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "currency-conversion-and-exchange-rates.p.rapidapi.com");
        }

        public async Task<double> ConvertCurrencyAsync(double amount, Currency sourceCurrency, Currency targetCurrency)
        {
            string uri = $"https://currency-conversion-and-exchange-rates.p.rapidapi.com/convert?from={sourceCurrency}&to={targetCurrency}&amount={amount}";

            using (var response = await _httpClient.GetAsync(uri))
            {
                response.EnsureSuccessStatusCode();

                var contentStream = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contentStream);

                ConversionResponseDTO ConversionResponseDTO =  JsonConvert.DeserializeObject<ConversionResponseDTO>(contentStream);

                var result = ConversionResponseDTO.Result;

                return result;
            }
        }
    }
  
        public class ConversionResponseDTO
        {
            public DateTime Date { get; set; }
            public Info Info { get; set; }
            public Query Query { get; set; }
            public double Result { get; set; }
            public bool Success { get; set; }
        }

        public class Info
        {
            public decimal Rate { get; set; }
            public long Timestamp { get; set; }
        }

        public class Query
        {
            public decimal Amount { get; set; }
            public string From { get; set; }
            public string To { get; set; }
        }
    }


