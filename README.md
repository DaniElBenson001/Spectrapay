# SpectraPay Documentation

## Overview
SpectraPay is an attempt to replicate the core functionalities of PayPal, focusing on virtual peer-to-peer (P2P) wallet transactions. It is developed using C# and .NET and allows users to make virtual payments, manage their balances, and view transaction histories. This documentation outlines the **functional requirements**, **solution architecture**, **response structure**, and other implementation details for SpectraPay.

## Functional Requirements

### 1. Signup and Login
- **Description**: Users can sign up using a username and password. User data is stored securely in the database.
- **Endpoints**:
  - `POST /api/auth/signup`: Registers a new user with username and password.
  - `POST /api/auth/login`: Authenticates the user and generates a JWT token for access.

### 2. Token-based Authentication
- **Description**: The system employs JWT (JSON Web Token) for authentication. After a successful login, the system generates a token that users must include in the Authorization header for accessing protected API endpoints.
- **Endpoint**:
  - `POST /api/auth/login`: Returns a JWT token upon successful login.

### 3. Virtual Payments
- **Description**: Users can transfer virtual money to another user by specifying the recipient and the amount.
- **Endpoints**:
  - `POST /api/payments/transfer`: Initiates a virtual transfer from the payer's account to the recipient's account.

### 4. Process Payments Locally
- **Description**: All payments are processed locally using virtual money. No external payment gateways are involved. Balances are updated locally after each transaction.
- **Endpoints**:
  - `POST /api/payments/transfer`: Processes and completes the virtual payment, updating balances.

### 5. View Transactions
- **Description**: Users can view a list of their past transactions, including sent and received payments.
- **Endpoints**:
  - `GET /api/transactions`: Retrieves a list of all transactions made or received by the user.

### 6. Detailed Transaction Info
- **Description**: Users can view detailed information about a specific transaction, such as the date, amount, and status.
- **Endpoints**:
  - `GET /api/transactions/{transactionId}`: Fetches details of a specific transaction.

### 7. Refund Payment
- **Description**: Users can request a refund for recent transactions. Virtual money is transferred back to the original payer.
- **Endpoints**:
  - `POST /api/payments/refund`: Initiates a refund for a specified transaction.

### 8. View Balance
- **Description**: Users can check their current virtual balance at any time.
- **Endpoints**:
  - `GET /api/account/balance`: Retrieves the current balance for the authenticated user.

### 9. Update Balance After Transaction
- **Description**: The system automatically updates the user’s balance after each transaction, whether it’s a payment or a refund.

### 10. Virtual Notifications
- **Description**: Users receive notifications for successful payments or received payments using a local notification system.

---

## Solution Architecture

SpectraPay follows a clean, scalable architecture with a separation of concerns between **Models**, **Services**, and the **Main Application**. The architecture is designed to replicate PayPal's core transactional functionalities.

### Folder Structure

```plaintext
SpectraPay
│
├── SpectraPay.Core          // Contains core business logic
│   ├── Models               // Entity Models (User, Transaction, Account)
│   ├── Services             // Business Services (UserService, PaymentService, TransactionService)
│
├── SpectraPay.Infrastructure // Infrastructure for data access and configuration
│   ├── Data                 // Entity Framework DB Context and Migrations
│   ├── Repositories         // Data Repositories for interacting with the database
│
├── SpectraPay.WebAPI         // Main application project for exposing API endpoints
│   ├── Controllers          // API Controllers (AuthController, PaymentController, TransactionController)
│   ├── Middleware           // JWT Authentication Middleware
│
├── SpectraPay.Tests          // Unit and integration tests
│
└── SpectraPay.sln            // Solution file
```

### Response Structure

```plaintext
{
  Status: boolean,
  StatusMessage: string,
  Data: {...}
}


