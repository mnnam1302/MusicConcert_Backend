# Run docker
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d

# Application

## Service Identity
	- Create Organization => Event: OrganizationCreated

## Service Catalog
### Events
	- Create OrganizationInfo => Consume Event: OrganizationCreated

### Features