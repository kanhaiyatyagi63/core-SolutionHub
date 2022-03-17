using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace ST.SolutionHub.Managers
{
    public class ApiManager
    {
        public static T Get<T>(string baseUrl, string apiUrl, params KeyValuePair<string, string>[] parameters) where T : class
        {
            try
            {
                var client = new RestClient(baseUrl);
                var request = new RestRequest(apiUrl, Method.GET);

                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }
                // execute the request
                IRestResponse restResponse = client.Execute(request);
                var content = restResponse.Content; // raw content as string
                                                    // deserialize the json into an object
                var response = JsonConvert.DeserializeObject<T>(content);
                return response;
            }
            catch (Exception ex)
            {
                return (T)null;
            }
        }
    }
}
