using Azure;
using Azure.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace TiVi.MLService.Utilities
{
    public class RestHelper<T>
    {
        private static RestClient _restClient = new RestClient();

        public static T Submit(Method httpMethod, string endpointAPI, string token,string jsonBody = null)
        {
            var restRequest = new RestRequest(httpMethod);
            _restClient.BaseUrl = new Uri(endpointAPI);

            restRequest.AddHeader("accept", "text/plain");
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Content-Type", "application/json");

            if (!String.IsNullOrEmpty(jsonBody))
            {
                restRequest.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                //restRequest.AddStringBody(jsonBody, DataFormat.Json);
            }
            IRestResponse response = _restClient.Execute(restRequest);

            var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }

        public static async Task<T> SubmitAsync(Method httpMethod, string endpointAPI, string token, string jsonBody = null)
        {
            var restRequest = new RestRequest(endpointAPI, httpMethod);
            _restClient.BuildUri(restRequest);
            restRequest.AddHeader("accept", "text/plain");
            restRequest.AddHeader("Authorization", "Bearer " + token);
            restRequest.AddHeader("Content-Type", "application/json");

            if (!String.IsNullOrEmpty(jsonBody))
            {
                restRequest.AddParameter("application/json", jsonBody, ParameterType.RequestBody);
                //restRequest.AddStringBody(jsonBody, DataFormat.Json);
            }
            IRestResponse response = await _restClient.ExecuteAsync(restRequest);

            var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }
    }
}
