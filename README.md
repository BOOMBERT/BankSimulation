# 🔹 Description
The __Bank Simulation__ is an __API__ that allows the simulation of __banking operations__, offering a __range of features__ for customers and administrators.
It also includes __currency exchange__ during financial transactions and __error logging__.

## 🔹 Information
This project uses an __external currency exchange API__ available at: *[https://www.exchangerate-api.com](https://www.exchangerate-api.com)*. 
The __access key__ is included in the *`appsettings.json`* file but __may not work__. 
If this happens, please __visit__ the API provider's website to __generate__ a new key.

## 🔹 Features
- __*Customer:*__
	- View personal data
	- Change password using the current password
	- Change email using the current email
	- Retrieve security question
	- Change password by answering the security question
	- Make a transfer
	- View details of all owned bank accounts
	- Access details of a specific bank account

- __*Admin:*__
	- Register an account for a customer
	- Retrieve customer data using the customer's ID
	- Change customer data using the customer's ID
	- Partially change customer data using the customer's ID
	- Delete a customer using the customer's ID (soft delete)
	- Retrieve customer data using the customer's email address
	- Retrieve customer data using the customer's bank account number
	- Set a security question for a customer using the customer's ID
	- Retrieve the customer's security question using the customer's ID
	- Change the customer's security question using the customer's ID
  	- Delete the customer's security question using the customer's ID
	- Deposit money into a customer's bank account
	- Withdraw money from a customer's bank account
	- Create a bank account for a customer
	- Retrieve a customer's bank accounts using the customer's ID
	- Retrieve a specific bank account using the customer's ID and bank account number
	- Delete a specific bank account using the customer's ID and bank account number (soft delete)

- __*Everyone:*__
	- Register an administrator account (credentials from environment variables)
	- Log in to the account (retrieve access token and refresh token)
	- Refresh the access token using the refresh token

## 🔹 Endpoints preview
![banksimulation-swagger-ui-screenshot](https://github.com/user-attachments/assets/ee09d3d7-1ba2-47a1-bf7c-76bfc6606080)

## 🔹 Database diagram
![banksimulation-database-diagram-screenshot](https://github.com/user-attachments/assets/0ec40669-1e34-4900-8cd8-a87b975dd2ff)

## 🔹 Run locally
- __*Prerequisites:*__
	- Git
	- Docker

- __*Setup:*__
	- *1. Clone the repository:*
		```bash		
	    git clone https://github.com/BOOMBERT/BankSimulation.git
		```
	- *2. Navigate to the project directory:*
		```bash			
	    cd BankSimulation
		```
	- *3. Build and run the API:*
		```bash	
	    docker-compose up --build
		```

- __*Access the API:*__
	- Swagger UI: *[https://localhost:8081/swagger/index.html](https://localhost:8081/swagger/index.html)* or *[http://localhost:8080/swagger/index.html](http://localhost:8080/swagger/index.html)*
	- Send a request to the specific endpoint
