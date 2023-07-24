# Bank

Some new stakeholders have requested us to implement a new, top of the range banking system! We plan to revolutionise the industry with Blockchain-enabled IoT and AI, powering a VR user interface that is backed by K8s and GoLang serverless APIs!

Firstly though, we've met with the stakeholders and discussed with them what the MVP will look like. In order to validate our work against these requirements, we've derived some acceptance criteria from the requirements.

Your task in this kata in to implement the MVP of the banking system. You have been provided with some requirements and acceptance criteria. You should use these acceptance criteria in conjunction with Outside-In Test-Driven Develpment to develop this system.

It is suggested to read through the requirements, and then work through each of the acceptance criteria from top to bottom. Remember to follow the double loop method of Outside-In TDD, and remember to focus on implementing only what you need for those tests to pass.

## Requirements

- The system should be able to support the capability of clients depositing and withdrawing money from their accounts.
- Clients should be able to view the balance.
- All money should be assumed to be in a single currency.
- For the MVP, client data is not needed to be persisted out of the lifetime of the system, but an in-memory database should be used.

## Acceptance Criteria

### Displaying balance

---

#### Scenario Outline

`As a client of the bank, I want to be able to view my account balance, so that I can know how much money is in my account`

#### Given, When, Then

```
GIVEN a client opens a new account
WHEN the client views their balance
THEN the client should have an account balance of 0
```

---

```
GIVEN a client has an existing account with a balance of 1000
WHEN the client views their balance
THEN the client should have an account balance of 1000
```

### Depositing money into an account

---

#### Scenario Outline

`As a client of the bank, I want to be able to deposit my money, so that I can store my money safely when I am not using it`

#### Given, When, Then

```
GIVEN a client opens a new account
WHEN the client makes a deposit of 1000
THEN the client should have a new account balance of 1000
```

---

```
GIVEN a client opens a new account
WHEN the client makes a deposit of 2000
THEN the client should have a new account balance of 2000
```

---

```
GIVEN a client has an existing account with a balance of 500
WHEN the client makes a deposit of 150
THEN the client should have a new account balance of 650
```

---

```
GIVEN a client opens a new account
WHEN the client makes a deposit of 500
AND the client makes a deposit of 200
THEN the client should have a new account balance of 700
```

```
GIVEN a client opens a new account
WHEN the client makes a deposit of 0.50
THEN the client should have a new account balance of 0.50
```

---

```
GIVEN a client opens a new account
WHEN the client makes a deposit of 2,150,000,000
THEN the client should have an account balance of 2,150,000,000
```

---

```
GIVEN a client opens a new account
WHEN the client makes a deposit of 3,000,000,001
THEN the deposit should be rejected
AND the client should be told that they can not make the deposit, due to exceeding the maximum account balance of 3,000,000,000
```

### Withdrawing money from an account

---

#### Scenario Outline

`As a client of the bank, I want to be able to withdraw my money from my account, so that I can access my money easily when I need to use it`

#### Given, When, Then

```
GIVEN a client has an existing account with a balance of 300
WHEN the client makes a withdrawal of 200
THEN the client should have a new account balance of 100
```

---

```
GIVEN a client has an existing account with a balance of 100
WHEN the client makes a withdrawal of 200
THEN the client should have a new account balance of -100
```

---

```
GIVEN a client opens a new account
WHEN the client makes a withdrawal of 1001
THEN the withdrawal should be rejected
AND the client should be told they can not exceed overdraft of 1000
```

---

## Stretch Goals

If you complete the acceptance criteria above, try using Outside-In TDD to implement these new requirements:

### Transferring funds between clients

---

#### Scenario

`As a client of the bank, I want to be able to transfer money to another client of the bank, so that we can easily to business with each other clients and manage our payments easily`

#### Given, When, Then

```
GIVEN Client A has an account with a balance of 1000
AND Client B has a balance of 5000
WHEN Client A transfers 500 to client B
THEN Client A should have a balance of 500
AND Client B should have a balance of 5500
```

---

```
GIVEN Client A has an account with a balance of 1000
AND Client B has an account with a balance of 2,999,999,999
WHEN Client A transfers 2 to client B
THEN the transfer should be rejected
AND Client A should be told that Client B can not accept the transaction because of how rich they are
```

### Viewing bank statements

---

#### Scenario

`As a client of the bank, I want to be able to view my statements and series of transactions, so I can understand the activity of my monetary assets in more detail`

#### Given, When, Then

```
GIVEN a client opens a new account
AND the client made a deposit of 1000 on 10-01-2012
AND the client made a deposit of 2000 on 13-01-2012
AND the client made a withdrawal of 500 on 14-01-2012
When the client prints her bank statement
Then the client would see the bank statement
```

| Date       | Credit  | Debit  | Balance |
| ---------- | ------- | ------ | ------- |
| 14/01/2012 | 0.00    | 500.00 | 2500.00 |
| 13/01/2012 | 2000.00 | 0.00   | 3000.00 |
| 10/01/2012 | 1000.00 | 0.00   | 1000.00 |

_Tip: For asserting against particularly complex response outputs, consider storing the expecting output in a text file and reading that into your test for the assertion._
