using Project.ViewModels.Vndirects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.API.Proxy.Interfaces
{
    public interface IVndirectInterface
    {
        Task<dynamic> Recommendations(RecommendationsRequest request);
        Task<dynamic> StrategyDetail(string request);
    }
}
