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
    public class ApiUtilities : IAPIClient
    {
       readonly RestClient restClient;
        public ApiUtilities(string baseUrl) {

            restClient = new RestClient(baseUrl);

        }

        public void SetBaseEndPoint(string baseUrl)
        {
            restClient.BaseUrl = new Uri(baseUrl);
        }
        public IRestResponse createAccount<T>(T payload) where T : class
        {
            var request = new RestRequest(Endpoints.CREATE_USER, Method.POST);
            request.AddBody(payload);
            return restClient.Execute<RestResponse>(request);
        }

        public IRestResponse deleteAccount<T>(T payload) where T : class
        {
            var request = new RestRequest(Endpoints.DELETE_USER, Method.DELETE);
            request.AddBody(payload);
            return restClient.Execute<RestResponse>(request);
        }

        public IRestResponse depositAmount<T>(T payload) where T : class
        {
            var request = new RestRequest(Endpoints.DEPOSIT_AMOUNT, Method.POST);
            request.AddBody(payload);
            return restClient.Execute<RestResponse>(request);
        }

        public IRestResponse getAccountDetails(string accountNumber)
        {
            var request = new RestRequest(Endpoints.GET_ACCOUNT_DETAILS, Method.GET);
           // request.AddUrlSegment(accountNumber);
            return restClient.Execute<RestResponse>(request);
        }

        public IRestResponse withdrawAmount<T>(T payload) where T : class
        {
            var request = new RestRequest(Endpoints.WITHDRAW_AMOUNT, Method.POST);
            request.AddBody(payload);
            return restClient.Execute<RestResponse>(request);
        }

        public DTO GetContent<DTO>(IRestResponse response)
        {
            var content = response.Content;
            DTO dto = JsonConvert.DeserializeObject<DTO>(response.Content);
            return dto;
        }

        public IRestResponse getAccountDetails<T>(string accountNumber) where T : class
        {
            throw new NotImplementedException();
        }

        public IRestResponse withdrawAmmount<T>(T payload) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
