# Run docker
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d

# Application

## Service Identity
### Organization
	- api/v1/organizations				Create Organization		=> Event: OrganizationCreated	OK

### Employee
	- api/v1/emplpyees					Create Employee			=> Event: EmployeeCreated		OK

### Customer
	- api/v1/customers					Create Customer			=> Event: CustomerCreated

### Authentication
	- api/v1/auth/employee/sign-in		Employee Login											OK
	- api/v1/auth/employee/sign-out		Employee Logout											OK

### Token
	- api/v1/tokens/employees/refresh-token			Refresh Token								OK

## Service Catalog
### Events
	- Create OrganizationInfo	=> Consume Event: OrganizationCreated

### Features