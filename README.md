<p align="center">
  <a href="#bulb-about">Concert Music</a>;
</p>

# Run docker infrastructure enviroment
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d

# Application
## Service Identity
| Entity	|				Features				|     	Event			|
|---------------|-----------------------------------------------------------------------|------------------------------	|
| Organization	| Create Organization 	- api/v1/organizations				|	OrganizationCreated	|
| Employee	| Create Employee 	- api/v1/employees				|				|
| Customer	| Create Customer 	- api/v1/customers				|	CustomerCreated		|
| Authenticate	| Sign-In Employee 	- api/v1/auth/employee/sign-in			|				|
| 		| Sign-Out Employee 	- api/v1/auth/employee/sign-out			|				|
| 		| Sign-In Customer 	- api/v1/auth/customer/sign-in			|				|
| 		| Sign-In Employee 	- api/v1/auth/customer/sign-out			|				|

## Service Catalog
