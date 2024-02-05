using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace BankingTransactionProject.Utilities
{
    public class Endpoints
    {
        public static readonly string CREATE_USER = "/posts/create";
        public static readonly string DELETE_USER = "/posts/delete";
        public static readonly string WITHDRAW_AMOUNT = "/posts/witdraw";
        public static readonly string DEPOSIT_AMOUNT = "/posts/deposit";
        public static readonly string GET_ACCOUNT_DETAILS = "/posts/account/{accountnumber}";

    }
}
