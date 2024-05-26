# Run docker
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d

# Application

## Service Identity
### Organization
	- Create Organization		=> Event: OrganizationCreated

### Employee
	- Create Employee			=> Event: EmployeeCreated

### Identity
	- Employee Login

## Service Catalog
### Events
	- Create OrganizationInfo	=> Consume Event: OrganizationCreated

### Features