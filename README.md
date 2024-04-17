## A full-stack transaction-tracker app made in ASP.NET Web API Core and Angular.

### Description
This project was programmed in C# and TypeScript. It has CRUD capabilites and allows users to log transactions. In addition, it has user registration, login, and logout with authentication implemented via JWS Bearer tokens.

Key features:
- Registration/Login/Logout: Allows for user creation and management with authentication.
- Transaction Management: CRUD functionality for transactions logged by the user that have a name, amount, category, and date.
- Data Display: Shows a breakdown of total spending by category, as well as the user's total spending and largest transaction.
  
## Installation/Congfiguration
Install an SQL server of your choice. 

Within your SQL server, create a database called "Transactions" and obtain its connection string.

Install .NET. You can find it [here](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net80)

Install Node.Js. You can find it [here](https://nodejs.org/en)

Install Angular and set execution policy.
```
npm install -g @angular/cli
```
```
Set-ExecutionPolicy -Scope CurrentUser -ExecutionPolicy RemoteSigned
```

Clone the the repository.
```
git clone https://github.com/JasonComo/transaction-app.git
```

Navigate to the backend's appsettings.json and change the "DefaultConnection" to your database's connection string.
```
cd transaction-app/TransactionAPI/appsettings.json
```
Navigate to Migrations and update the database. 
```
cd transaction-app/TransactionAPI/Migrations
```
```
dotnet ef database update
```
## Usage
Navigate back to the backend and run the API.
```
cd transaction-app/TransactionAPI
```
```
dotnet run
```

Open a new terminal, and navigate to back to the repository, and to the frontend.
```
cd transaction-app/TransactionAngular
```

Run the Angular application.
```
ng serve -o
```

## Register
Click the "Register" button to create an account.

## Login
After creating an account, click the "Back to Login" button and login by entering your username and password. 

## Transactions
Create a transaction by entering the information in the form and clicking the "Create Transaction" button. Edit and/delete by clicking the pencil or trash can icons below the transaction.

### Acknowledgements
A portion of the code used for user authentication and Bearer token setup was borrowed from a tutorial from NetCode. The corresponding repository can be found [here](https://github.com/Netcode-Hub/DemoCustomJwtDotNet8BlazorWasmAuthAndAuthWithIdentityManagerSolution)






