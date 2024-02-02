using System;
using TechTalk.SpecFlow;
using Newtonsoft.Json;
using BankingTransactionProject.Utilities;
using RestSharp;
using NUnit.Framework;
using BankingTransactionProject.Modals;
using Newtonsoft.Json.Linq;
using RestSharp.Serialization.Json;

namespace BankingTransactionProject.StepDefinitions
{
    [Binding]
    public class BankingSystemAPIStepDefinitions
    {

        private readonly ApiUtilities apiUtilities;
        private AccountDetails accountDetails;
        private IRestResponse response;

        public BankingSystemAPIStepDefinitions()
        {
            apiUtilities = new ApiUtilities("http://localhost:3000/");
            accountDetails = new AccountDetails();
            response = new RestResponse();
        }


        [Given(@"the API endpoint for user accounts is ""(.*)""")]
        public void GivenTheAPIEndpointForUserAccountsIs(string endpoint)
        {
            apiUtilities.SetBaseEndPoint(endpoint);
        }

        [When(@"I send a POST request to create an account for the user with ID ""(.*)"" and account type ""(.*)""")]
        public void WhenISendAPOSTRequestToCreateAnAccountForTheUserWithIDAndAccountType(string userID, string accountType)
        {
            var accountDetails = new AccountDetails { UserId = userID, AccountType = accountType };
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(accountDetails);
            response = apiUtilities.SendPostRequest("posts", payload);
        }

        [When(@"I send a POST request to create an account for the account with number ""(.*)"" and account type ""(.*)""")]
        public void WhenISendAPOSTRequestToCreateAnAccountForAccountWithNumber(string number, string accountType)
        {
            var accountDetails = new AccountDetails { AccountNumber = number, AccountType = accountType };
            string payload = Newtonsoft.Json.JsonConvert.SerializeObject(accountDetails);
            response = apiUtilities.SendPostRequest("posts", payload);
        }

        [When(@"I update the amount in the account as (.*) for account with number ""(.*)""")]
        public void updateAmountInAccount(int amount, string accountNumber)
        {
            var accountDetails = new AccountDetails { Amount = amount, AccountNumber = accountNumber };
            string payload = JsonConvert.SerializeObject(accountDetails);
            response = apiUtilities.depositAmount(payload);
        }


        [Then(@"I verify the amount (.*) is (.*) for account number ""(.*)""")]
        public void ThenIVerifyTheAmountIsUpdatedForAccountNumber(int amount, string state, string accountNumber)
        {
            if (state.Equals("updated"))
            {
                response = apiUtilities.getAccountDetails(accountNumber);
                // Assert.AreEqual(amount, response.accountNumber);
            }
            else
            {

            }
        }



        [When(@"I send a GET request to retrieve accounts for the user with ID ""(.*)""")]
        public void WhenISendAGETRequestToRetrieveAccountsForTheUserWithID(string userID)
        {
            response = apiUtilities.SendGetRequest($"posts?userId={userID}");

        }

        [Then(@"the response status code should be (.*)")]
        public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, (int)response.StatusCode);
        }

        [Then(@"the response should contain both accounts with types ""(.*)"" and ""(.*)"" for userID ""(.*)""")]
        public void ThenTheResponseShouldContainBothAccountsWithTypesAndForUserID(string accountType1, string accountType2, string userID)
        {
            JArray accounts = JArray.Parse(response.Content);
            Assert.IsTrue(accounts.Any(a => a["accountType"].ToString() == accountType1), "Account is creared successfully");
            Assert.IsTrue(accounts.Any(a => a["accountType"].ToString() == accountType2), "Account is creared successfully");

        }

        [When(@"I send a GET request to retrieve accounts for the user with account type ""(.*)""")]
        public void WhenISendAGETRequestToRetrieveAccountsForTheUserWithAccountType(string accountType)
        {
            response = apiUtilities.SendGetRequest($"posts?AccountType={accountType}");
        }

        [When(@"I send a DELETE request to delete an account for the user with account type ""(.*)""")]
        public void WhenISendAFDeleteRequestTodeleteanAccountsForTheUserWithAccountType(string accountType)
        {
            response = apiUtilities.SendGetRequest($"posts?AccountType={accountType}");
            JArray users = JArray.Parse(response.Content);
            foreach (JObject item in users)
            {
                string id = (string)item["id"];
                response = apiUtilities.SendDeleteRequest($"posts/{id}");
            }

        }

        [When(@"I send a DELETE request to delete an account for invalid user with ID ""(.*)""")]
        public void WhenISendAFDeleteRequestTodeleteanAccountsForInvalidUser(string userID)
        {
            response = apiUtilities.SendDeleteRequest($"posts/{userID}");

        }
        [When(@"I create an account with details as (.*), ""(.*)"", ""(.*)""")]
        public void WhenICreateAnAccountWithDetailsAs(int accountBalance, string accountHolder, string accountType)
        {
            var accountDetails = new AccountDetails { AccountBalance = accountBalance, AccountHolder = accountHolder, AccountType = accountType };
            string payload = JsonConvert.SerializeObject(accountDetails);
            response = apiUtilities.createAccount(payload);
            //  response = apiUtilities.SendPostRequest("posts", payload);
        }

        [Then(@"I verify the account (.*) for (.*), ""(.*)"", ""(.*)""")]
        public void ThenIVerifyTheAccountCreation(String state, int accountBalance, string accountHolder, string accountType)
        {
           
            if (state == "created")
            {
                response = apiUtilities.getAccount($"posts?accountHolder={accountHolder}");
                Assert.AreEqual(200, (int)response.StatusCode);
                var deserialize = new JsonDeserializer();
                var output = deserialize.Deserialize<Dictionary<string, string>>(response);
                var result = output["accountBalance"];
                Assert.That(result, Is.EqualTo(accountBalance));
            }
            else
            {
                Assert.AreEqual(400, (int)response.StatusCode);

            }
        }
    }
}