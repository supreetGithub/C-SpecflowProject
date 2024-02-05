using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace BankingTransactionProject.Utilities
{
    public interface IAPIClient
    {
        IRestResponse createAccount<T>(T payload) where T : class;
        IRestResponse depositAmount<T>(T payload) where T : class;
        IRestResponse deleteAccount<T>(T payload) where T : class;
        IRestResponse withdrawAmmount<T>(T payload) where T : class;
        IRestResponse getAccountDetails<T>(string accountNumber) where T : class;

    }
}
