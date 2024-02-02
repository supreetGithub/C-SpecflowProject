Feature: Banking System API

Background: 
Given the API endpoint for user accounts is "http://localhost:3000/"


@multipleaccounts
Scenario: User can have multiple accounts
When I send a POST request to create an account for the user with ID "123" and account type "Savings"
And I send a POST request to create an account for the user with ID "123" and account type "Current"
And I send a GET request to retrieve accounts for the user with ID "123"
Then the response status code should be 200
And the response should contain both accounts with types "Savings" and "Current" for userID "123"

@DeleteValidAccount
Scenario: Postive : Delete Valid accounts
When I send a POST request to create an account for the user with ID "123" and account type "Joint"
And I send a GET request to retrieve accounts for the user with account type "Joint"
Then the response status code should be 200
When I send a DELETE request to delete an account for the user with account type "Joint"
Then the response status code should be 200


@DeleteInvalidAccount
Scenario: Negative-  Delete Valid accounts
When I send a DELETE request to delete an account for invalid user with ID "6474"
Then the response status code should be 404



@Depositvalidamount
Scenario: Deposit Amount
When I send a POST request to create an account for the account with number "123854895" and account type "Savings"
Then the response status code should be 200
When I update the amount in the account as 3000 for account with number "123854895"
Then I verify the amount 3000 is updated for account number "123854895"

@Depositnegativeamount
Scenario: Deposit Negative Amount
When I send a POST request to create an account for the account with number "123854895" and account type "Savings"
Then the response status code should be 200
When I update the amount in the account as -3000 for account with number "123854895"
Then I verify the amount -3000 is notupdated for account number "123854895"
And  the response status code should be 400

@Depositabovelimit
Scenario: Deposit above limit
When I send a POST request to create an account for the account with number "123854895" and account type "Savings"
Then the response status code should be 200
When I update the amount in the account as $12000 for account with number "123854895"
Then I verify the amount $12000 is notupdated for account number "123854895"
And  the response status code should be 400


@withdraw
Scenario: Withdraw Amount
When I send a POST request to create an account for the account with number "123854895" and account type "Savings"
Then the response status code should be 200
When I withdraw the amount in the account as 3000 for account with number "123854895"
Then I verify the amount 3000 is updated for account number "123854895"

@CreatewithValiddata
Scenario Outline: Create Account with valid data
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I verify the account created for <Balance>, "<AccountHolder>", "<AccountType>"
Examples: 
| Balance | AccountHolder | AccountType |
| 2000    | Test          | Savings     |
| 3000    | TestABC       | Current     |



@Createwithinvaliddata
Scenario Outline: Create Account with invalid data
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 400
And I verify the account not created for <Balance>, "<AccountHolder>", "<AccountType>"
Examples: 
| Balance | AccountHolder | AccountType |
| -2000    | Test          | Savings     |








