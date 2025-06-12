using Azure.Core;
using Newtonsoft.Json.Linq;
using Project.API.Proxy.Interfaces;
using ZaloDotNetSDK;
namespace Project.API.Proxy
{
    public class ZaloService : IZaloService
    {
     
        public dynamic GetFriends()
        {
                long appId = 700059110068352392;
             string secretKey = "mXeUGFiuXEUj1J12QLkf";
        ZaloAppInfo appInfo = new ZaloAppInfo(appId, secretKey, "callbackUrl");
            ZaloAppClient appClient = new ZaloAppClient(appInfo);

            string accessToken = "qh3bDIbiqsJJiU0yBcNGOTUwwZio8eiNjuZd1oWMqpNKggGuJHND5VJDcZLm5guPq8_A25K5kmdhkuqL4JEz6ec9-IO78hbyeFRbOXfKiJgSv_4a3cEvVRAyzo81A9KzkORTE14vZXgNdimNDII5FAofr0TJ6kGihhsMSnq6fJQNhU4IBmkL5Toe_myc2hSTgOhCG201-c-0hx4n53J00BpfeGK3LDeP-EoiHNbCm1pczPmuMd3gQCJRe64xHUbMii-B1XnsmHRlrer8HLFcSzVprczGRhzgrkJHTtLHX726rVLT9dkINQNxgM0vGy93aVo7PXrYutIvvOrc1bxC8xd6lID2PSep--s5EbDawm2ikDXR8NdkMili0sWjSj5j";
            JObject friends =  appClient.getFriends(accessToken, 0, 10, "fields");

            return friends;
        }

        public Task<string> GetToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
