using Newtonsoft.Json;
using RestSharp;

namespace checkout.com.api.Integrations
{
    public class WebExtension<T>
    {
        public async Task<bool> MakeRequest(string basePath, string endPoint, Method method, string content) 
        {
            var client = new RestClient(basePath);
            var request = new RestRequest(endPoint, method);
            request.AddHeader("Accept", "application/json");
            request.AddBody(content);
            var result = await client.ExecuteAsync(request);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }
            var returnResult = JsonConvert.DeserializeObject(result.Content);
            return true;
        }
    }
}
