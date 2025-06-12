using Project.API.Proxy.Interfaces;
using Project.ViewModels.Vndirects;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Project.API.Proxy
{
    public class VndirectService : IVndirectInterface
    {
        private readonly string _api;

        public VndirectService(string api)
        {
            _api = api;
        }
        public async Task<dynamic> Recommendations(RecommendationsRequest query)
        {
            try
            {
                var url =  $"https://finfo-api.vndirect.com.vn/v4/recommendations?size=100&page=1";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();

                return res;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<dynamic> StrategyDetail(string stock )
        {
            try
            {
                var url = "https://apipubaws.tcbs.com.vn" + $"/tcai/v1/backtestv2/recomm/strategy-detail?ticker={stock}&strategyKey=price_volume_increase";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var res = await response.Content.ReadAsStringAsync();

                return res;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
