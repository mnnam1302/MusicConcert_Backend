<p align="center">
  <a href="#bulb-about">Concert Music</a>
</p>

# Run docker infrastructure enviroment
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d

# Application
## Entity Features and Events

| Entity        | Action           | Url                                                | Event - Queue          | Status  |
|---------------|------------------|----------------------------------------------------|------------------------|---------|
| Organization  | Create           | api/v1/organizations                               | organization-created   | pass    |
|               | Delete           | api/v1/organizations/{organizationId}              | organization-deleted   | pass    |
| Employee      | Create           | api/v1/employees                                   | employee-created       | pass    |
|               | Delete           | api/v1/employees/{employeeId}                      | employee-deleted       | pass    |
| Customer      | Create           | api/v1/customers                                   | customer-created       | pass    |
|               | Delete           | api/v1/customers/{customerId}                      | customer-deleted       | pass    |
| Authenticate  | Sign-In Employee | api/v1/auth/employee/sign-in                       |                        | pass    |
|               | Sign-Out Employee| api/v1/auth/employee/sign-out                      |                        | pass    |
|               | RefreshToken     | api/v1/auth/employee/refresh                       |                        | pass    |
|               | Sign-In Customer | api/v1/auth/customer/sign-in                       |                        | pass    |
|               | Sign-Out Customer| api/v1/auth/customer/sign-out                      |                        | pass    |
|               | RefreshToken     | api/v1/auth/customer/refresh                       |                        | pass    |

## Service Catalog

| Entity        | Action           | Url                                                | Event - Queue          | Status  |
|---------------|------------------|----------------------------------------------------|------------------------|---------|
| Category      | Create           | api/v1/categories                                  |                        | pass    |
|               | Get              | api/v1/categories                                  |                        | pass    |
|               | Get              | api/v1/categories/{categoriesId}                   |                        | pass    |
|               | Update           | api/v1/categories/{categoriesId}                   |                        | pass    |
|               | Delete           | api/v1/categories/{categoriesId}                   |                        | pass    |
| Event		    | Create           | api/v1/events										|                        | pass    |
|               | Get              | api/v1/events										|                        | pass	   |
|               | Get              | api/v1/events/{eventId}							|                        | pass	   |
|               | Update           | api/v1/events/{eventId}							|                        | pass	   |
|               | Delete           | api/v1/events/{eventId}							|                        | pass	   |
| Ticket		| Create           | api/v1/tickets										|                        |         |
|               | Get              | api/v1/tickets										|                        |   	   |
|               | Get              | api/v1/tickets/{ticketId}							| ticket-created         |   	   |
|               | Update           | api/v1/tickets/{ticketId}							| ticket-updated         |  	   |
|               | Delete           | api/v1/tickets/{ticketId}							| ticket-deleted         |   	   |
