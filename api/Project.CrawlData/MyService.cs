using Microsoft.Extensions.Hosting;

namespace Project.CrawlData
{
    public class MyService : BackgroundService
    {
        private readonly string _domain;
        public MyService(string domain)
        {
            _domain = domain;
        }
        public async Task<dynamic> VndirectAsync()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, _domain + "/api/CrawlData/posts/list");
          
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine();
            var data = await response.Content.ReadAsStringAsync();
            Console.WriteLine(data);
            return response;
        }

        public async Task<dynamic> CrawlStocks()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, _domain + "/api/CrawlData/stocks/list");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine();
            var data = await response.Content.ReadAsStringAsync();
            Console.WriteLine(data);
            return response;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Starting");

            stoppingToken.Register(() =>
                Console.WriteLine("Stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Working");

                // Your code here
                await VndirectAsync();

                await CrawlStocks();

                await Task.Delay(TimeSpan.FromHours(5), stoppingToken);
            }

            Console.WriteLine($"Stopping.");
        }
    }
}
