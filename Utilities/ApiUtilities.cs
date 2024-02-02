using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace BankingTransactionProject.Utilities
{
    public class ApiUtilities
    {
        public RestClient client;
       public ApiUtilities(String baseUrl)
        {
            client = new RestClient(baseUrl);
        }

        public void SetBaseEndPoint(string baseUrl)
        {
          client.BaseUrl = new Uri(baseUrl);
        }

        public IRestResponse SendPostRequest(string endpoint, string payload) { 
            var request = new RestRequest(endpoint,Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json",payload,ParameterType.RequestBody);
            return client.Execute(request);
        }
        public IRestResponse SendGetRequest(string endpoint)
        {
            var request = new RestRequest(endpoint,Method.GET);
            return client.Execute(request);
        }

        public IRestResponse SendDeleteRequest(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.DELETE);
            return client.Execute(request);
        }

        public IRestResponse createAccount (String payload) {
            var request = new RestRequest("posts", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", payload, ParameterType.RequestBody);
            return client.Execute(request);
        }

        public IRestResponse depositAmount(String payload)
        {
            var request = new RestRequest("posts", Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", payload, ParameterType.RequestBody);
            return client.Execute(request);
        }

        public IRestResponse getAccount(string endpoint)
        {
            var request = new RestRequest(endpoint, Method.GET);
            return client.Execute(request);
        }
        public IRestResponse getAccountDetails(string accountNumber)
        {
            var request = new RestRequest(accountNumber , Method.GET);
            return client.Execute(request);
        }
    }
}
