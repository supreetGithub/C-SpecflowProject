using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
using BankingTransactionProject.Utilities;
using RestSharp;
using NUnit.Framework;
using BankingTransactionProject.Modals;

namespace BankingTransactionProject.StepDefinitions
{
    [Binding]
    public class BankingSystemAPIStepDefinitions
    {

        private readonly ApiUtilities apiUtilities;
        private accountDetailsRequest accountDetails;
        private IRestResponse response;
        private int initialBalance;
        private int withdrawlBalance;
        private int finalBalance;
        private string accountId;

        public BankingSystemAPIStepDefinitions()
        {
            apiUtilities = new ApiUtilities("http://localhost:3000/");
            accountDetails = new accountDetailsRequest();
            response = new RestResponse();
        }


        [Given(@"the API endpoint for user accounts is ""(.*)""")]
        public void GivenTheAPIEndpointForUserAccountsIs(string endpoint)
        {
            apiUtilities.SetBaseEndPoint(endpoint);
        }

        [When(@"I create an account with details as (.*), ""(.*)"", ""(.*)""")]
        public void WhenICreateAnAccountWithDetailsAs(int accountBalance, string accountHolder, string accountType)
        {
            var accountDetails = new accountDetailsRequest { AccountBalance = accountBalance, AccountHolder = accountHolder, AccountType = accountType };
            string payload = JsonConvert.SerializeObject(accountDetails);
            response = apiUtilities.createAccount(payload);
            accountDetailsResponse content = apiUtilities.GetContent<accountDetailsResponse>(response);
            accountId = content.AccountID;
        }

        [Then(@"I verify the account (.*) for (.*), ""(.*)"", ""(.*)""")]
        public void ThenIVerifyTheAccountCreation(String state, int accountBalance, string accountHolder, string accountType)
        {
            if (state == "created")
            {
                response = apiUtilities.getAccountDetails(accountId);
                Assert.AreEqual(200, (int)response.StatusCode);
                accountDetailsResponse content = apiUtilities.GetContent<accountDetailsResponse>(response);
                Assert.Equals(content.AccountBalance, accountBalance);
                Assert.Equals(content.AccountHolder, accountHolder);
                Assert.Equals(content.AccountType, accountType);
            }
            else
            {
                Assert.AreEqual(400, (int)response.StatusCode);

            }
        }

        [When(@"I delete the account created for (.*)")]
        public void WhenIDeleteTheAccountCreatedFor(string accountType)
        {
            if (accountType.Equals("valid account"))
            {
                var accountDetails = new accountDetailsRequest { AccountType = accountType };
                string payload = JsonConvert.SerializeObject(accountDetails);
                response = apiUtilities.deleteAccount(payload);
                accountDetailsResponse content = apiUtilities.GetContent<accountDetailsResponse>(response);
                accountId = content.AccountID;
            }
            else
            {
                var accountDetails = new accountDetailsRequest { AccountType = accountType };
                string payload = JsonConvert.SerializeObject(accountDetails);
                response = apiUtilities.deleteAccount(payload);
                accountDetailsResponse content = apiUtilities.GetContent<accountDetailsResponse>(response);
                accountId = content.AccountID + "647";
            }

        }

        [Then(@"the account (.*) deleted successfully")]
        public void ThenTheAccountShouldBeDeletedSuccessfullyForAccountType(string state)
        {
            response = apiUtilities.getAccountDetails(accountId);
            if (state.Equals("is"))
            {
                if ((int)response.StatusCode == 400)
                {
                    Console.WriteLine("Account deleted successfully");
                }
                else
                {
                    throw new Exception("Account not deleted successfully");

                }
            }
            else
            {
                Assert.Equals((int)response.StatusCode, 200);
            }
        }
   
        [When(@"I withdraw \$(.*) in a single transaction")]
        public void WhenIWithdeawInASingleTransaction(int amount)
        {
            withdrawlBalance = amount;
            var amountToWithdraw = new accountDetailsRequest { withdrawlBalance = withdrawlBalance };
            string payload = JsonConvert.SerializeObject(amountToWithdraw);
            response = apiUtilities.withdrawAmount(payload);
        }


        [Then(@"the remaining balance in ""(.*)"" is now \$(.*)")]
        public void ThenTheRemainingBalanceInIsNow(string accountHolder, int amount)
        {
            accountDetailsResponse getContent = apiUtilities.GetContent<accountDetailsResponse>(response);
            finalBalance = getContent.AccountBalance;
            Assert.Equals(initialBalance-finalBalance,amount); 
        }

        [Then(@"the withdrawl should be (.*) and balance (.*) \$(.*)")]
        public void ThenTheWithdrawlShouldBeSuccessfulAndBalanceIsNot(string state, string valid, int balance)
        {
            initialBalance = balance;
            accountDetailsResponse withdrawlContent = apiUtilities.GetContent<accountDetailsResponse>(response);
            var accountNumber = withdrawlContent.AccountNumber;
            response = apiUtilities.getAccountDetails(accountNumber);
            accountDetailsResponse getContent = apiUtilities.GetContent<accountDetailsResponse>(response);
            finalBalance = getContent.AccountBalance;

            if (state.Equals("successful") && valid.Equals("is"))
            {
                if (initialBalance != finalBalance)
                {
                    Console.WriteLine("Withdrawl is successful");
                }
                else
                {
                    throw new Exception("Withdrawl failed!!!");
                }
            }
            else
            {
                if (initialBalance == finalBalance)
                {
                    Console.WriteLine("Decline is successful");
                }
                else
                {
                    throw new Exception("Decline failed!!!");
                }
            }
        }


        [Then(@"I deposit \$(.*) to the newly created account")]
        public void ThenIDepositToTheNewlyCreatedAccount(int amount)
        {
            var depositAmount = new accountDetailsRequest { Amount = amount};
            string payload = JsonConvert.SerializeObject(depositAmount);
            response = apiUtilities.depositAmount(payload);
        }

        [Then(@"I verify the balance is updated with the new amount")]
        public void ThenIVerifyTheBalanceIsUpdatedWithTheNewAmount()
        {
            accountDetailsResponse updatedAmount = apiUtilities.GetContent<accountDetailsResponse>(response);
            var balance = updatedAmount.AccountBalance;
            Assert.AreNotEqual(initialBalance, balance);

        }


        [Then(@"I verify the balance is not updated with the new amount")]
        public void ThenIVerifyTheBalanceIsNotUpdatedWithTheNewAmount()
        {
            response = apiUtilities.getAccountDetails(accountId);
            accountDetailsResponse updatedAmount = apiUtilities.GetContent<accountDetailsResponse>(response);
            var balance = updatedAmount.AccountBalance;
            Assert.AreEqual(initialBalance, balance);
        }

        [Then(@"the account is created for both accounts (.*) and (.*)")]
        public void ThenTheAccountIsCreatedForBothAccountsSavingsAndCurrent(string accountType1, string accountType2)
        {
            response = apiUtilities.getAccountDetails(accountId);
            accountDetailsResponse getContent = apiUtilities.GetContent<accountDetailsResponse>(response);
            Assert.AreEqual(getContent.AccountType, accountType1);
            Assert.AreEqual(getContent.AccountType, accountType2);
        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, (int)response.StatusCode);
        }
    }
}