using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingTransactionProject.Modals
{
    public class accountDetailsResponse
    {
        public string AccountID { get; set; }
        public string AccountType { get; set; }

        public int AccountBalance { get; set; }

        public string AccountHolder { get; set; }

        public string AccountNumber { get; set; }

    }
}
