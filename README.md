<p align="center">
  <a href="#bulb-about">Concert Music</a>;
</p>

# Run docker infrastructure enviroment
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d

# Application
## Service Identity
| Entity	|				Features				|     	Event			|
|---------------|-----------------------------------------------------------------------|------------------------------	|
| Organization	| Create  - api/v1/organizations				|	organization-created	|
|               | Delete  - api/v1/organizations/{organizationId}				|	organization-deleted	|
| Employee	| Create  - api/v1/employees				| employee-created		 |
|           | Delete  - api/v1/employees/{employeeId}				| employee-deleted    |
| Customer	| Create - api/v1/customers				|	customer-created		|
|           | Delete - api/v1/customers/{customerId}				|	customer-deleted		|
| Authenticate	| Sign-In Employee 	- api/v1/auth/employee/sign-in			|				|
| 		| Sign-Out Employee 	- api/v1/auth/employee/sign-out			|				|
| 		| RefreshToken Employee 	- api/v1/auth/employee/refresh			|				|
| 		| Sign-In Customer 	- api/v1/auth/customer/sign-in			|				|
| 		| Sign-In Customer 	- api/v1/auth/customer/sign-out			|				|
| 		| RefreshToken Customer 	- api/v1/auth/customer/refresh			|				|

## Service Catalog
| Entity	|				Features				|     	Event			|
|---------------|-----------------------------------------------------------------------|------------------------------	|
| Category	| Create  - api/v1/categories				|    	|
|           | Get  - api/v1/categories				|				|
|           | Get  - api/v1/categories/{categoriesId}				|				|
|           | Update  - api/v1/categories/{categoriesId}				|				|
|           | Delete  - api/v1/categories/{categoriesId}				|				|

