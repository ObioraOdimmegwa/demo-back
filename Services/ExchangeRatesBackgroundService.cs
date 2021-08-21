using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Server
{
    public class ExchageRatesBackgroundService : BackgroundService
    {
        private static string apiUrl = "https://rest.coinapi.io/v1/exchangerate/USD?invert=true";
        private static string apiKey = "59D31795-F71C-422D-A96E-EB767D100A3B";
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (HttpClient http = new HttpClient())
                    {
                        HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                        req.Headers.Add("X-CoinAPI-Key", apiKey);
                        HttpResponseMessage res = await http.SendAsync(req);
                        if(res.IsSuccessStatusCode)
                        {
                            ExchangeRatesResponse rates = JsonConvert.DeserializeObject<ExchangeRatesResponse>(await res.Content.ReadAsStringAsync());
                            Console.WriteLine(rates.rates[0].asset_id_quote);
                            ExchangeRates.UpdateRates(rates);
                        }
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(20));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}