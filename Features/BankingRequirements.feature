Feature: Banking System API

Background: 
Given the API endpoint for user accounts is "http://localhost:3000/"


@CreatewithValiddata
Scenario Outline: Create Account with valid information
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I verify the account created for <Balance>, "<AccountHolder>", "<AccountType>"
Examples: 
| Balance | AccountHolder | AccountType |
| 2000    | Test          | Savings     |
| 3000    | TestABC       | Current     |

@Createwithinvaliddata
Scenario Outline: Create Account with invalid information
When I create an account with details as <AccountBalance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 400
And I verify the account not created for <AccountBalance>, "<AccountHolder>", "<AccountType>"
Examples: 
| Balance | AccountHolder | AccountType |
| -2000    | Test          | Savings     |

@DeleteValidAccount
Scenario Outline: Delete Existing accounts
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I verify the account created for <Balance>, "<AccountHolder>", "<AccountType>"
When I delete the account created for valid account
Then the account is deleted successfully
Examples: 
| Balance | AccountHolder | AccountType |
| 2000    | TestDelete    | Joint     |


@DeleteInvalidAccount
Scenario Outline: Delete non existing accounts
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I verify the account created for <Balance>, "<AccountHolder>", "<AccountType>"
When I delete the account created for invalid account
Then the account is not deleted successfully
Examples: 
| Balance | AccountHolder | AccountType |
| $1000    | TestDeleteInvalid    | JointTest     |


@Wihdrawlimit
Scenario: User can withdraw up tp 90% of their  total balance in single transaction
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
When I withdraw $900 in a single transaction
Then the withdrawl should be successful and balance is not <Balance>
And the remaining balance in "<AccountHolder>" is now $100
Examples: 
| Balance | AccountHolder | AccountType |
| $1000    | TestWithdraw | Savings     |


@WithdrawAboveLimit
Scenario: User cannot withdraw up tp 90% of their  total balance in single transaction
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
When I withdraw $910 in a single transaction
Then the withdrawl should be declined and balance is <Balance>
And the remaining balance in "<AccountHolder>" is now $1000
Examples: 
| Balance | AccountHolder | AccountType |
| $1000    | TestWithdraw | Savings     |

@DepositValidAmount
Scenario: Deposit valid Amount
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I deposit $900 to the newly created account
Then I verify the balance is updated with the new amount
Examples: 
| Balance | AccountHolder | AccountType |
| $1000    | TestWithdraw | Savings     |

@Depositnegativeamount
Scenario: Deposit Negative Amount
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I deposit $-900 to the newly created account
Then the response status code should be 400
Examples: 
| Balance | AccountHolder | AccountType |
| $1000    | TestWithdraw | Savings     |


@Depositabovelimit
Scenario: Deposit amount above limit that is $10,000
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
And I deposit $12000 to the newly created account
Then the response status code should be 400
And I verify the balance is not updated with the new amount
Examples: 
| Balance | AccountHolder | AccountType |
| $1000    | TestWithdraw | Savings     |


@multipleaccounts
Scenario Outline: Single User with multiple accounts
When I create an account with details as <Balance>, "<AccountHolder>", "<AccountType>"
Then the response status code should be 200
When I create an account with details as <Balance1>, "<AccountHolder>", "<AccountType1>"
Then the response status code should be 200
And the account is created for both accounts <AccountType> and <AccountType2>
Examples: 
| Balance | AccountHolder | AccountType | Balance1 | AccountType2 |
| $1000   | TestWithdraw  | Savings     | $1500    | Current      |