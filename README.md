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
|               | Get              | api/v1/organizations                               |                        | pass    |
|               | Get              | api/v1/organizations/{organizationId}              |                        | pass    |
|               | Delete           | api/v1/organizations/{organizationId}              | organization-deleted   | pass    |
| Employee      | Create           | api/v1/employees                                   | employee-created       | pass    |
|               | Get              | api/v1/employees                                   |                        |         |
|               | Get              | api/v1/employees/{employeeId}                      |                        | pass    |
|               | Delete           | api/v1/employees/{employeeId}                      | employee-deleted       | pass    |
| Customer      | Create           | api/v1/customers                                   | customer-created       | pass    |
|               | Get              | api/v1/customers                                   |                        | pass    |
|               | Get              | api/v1/customers/{customerId}                      |                        | pass    |
|               | Delete           | api/v1/customers/{customerId}                      | customer-deleted       | pass    |
| Authenticate  | Sign-In Employee | api/v1/auth/employee/sign-in                       |                        | pass    |
|               | Sign-Out Employee| api/v1/auth/employee/sign-out                      |                        | pass    |
|               | RefreshToken     | api/v1/auth/employee/refresh                       |                        | pass    |
|               | Sign-In Customer | api/v1/auth/customer/sign-in                       |                        | pass    |
|               | Sign-Out Customer| api/v1/auth/customer/sign-out                      |                        | pass    |
|               | RefreshToken     | api/v1/auth/customer/refresh                       |                        | pass    |

## Service Catalog

| Entity              | Action           | Url                                                | Event - Queue          | Status  |
|---------------------|------------------|----------------------------------------------------|------------------------|---------|
| Category            | Create           | api/v1/categories                                  |                        | pass    |
|                     | Get              | api/v1/categories                                  |                        | pass    |
|                     | Get              | api/v1/categories/{categoriesId}                   |                        | pass    |
|                     | Update           | api/v1/categories/{categoriesId}                   |                        | pass    |
|                     | Delete           | api/v1/categories/{categoriesId}                   |                        | pass    |
| Event		          | Create           | api/v1/events									  |                        | pass    |
|                     | Get              | api/v1/events									  |                        | pass	 |
|                     | Get              | api/v1/events/{eventId}							  |                        | pass	 |
|                     | Update           | api/v1/events/{eventId}							  |                        | pass	 |
|                     | Delete           | api/v1/events/{eventId}							  |                        | pass	 |
| Ticket		      | Create           | api/v1/tickets									  | ticket-created         | pass	 |
|                     | Get              | api/v1/tickets									  |                        |   	     |
|                     | Get              | api/v1/tickets/{ticketId}						  |				           |   	     |
|                     | Delete           | api/v1/tickets/{ticketId}						  | ticket-deleted         | pass    |
|				      | Update           | 													  | stock-reversed         | pass	 |
|				      |                  | 													  | stock-reversed-failed  | pass	 |
|Consume Organization | Created          | 													  | organization-created   | pass	 |
|                     | Deleted          | 													  | organization-deleted   | pass  	 |
|Consume Order        |                  | 													  | order-created          | pass	 |

## Service Order

| Entity           | Action           | Url                                                 | Event - Queue            | Status  |
|------------------|------------------|-----------------------------------------------------|--------------------------|---------|
| Order	           | Create           | api/v1/orders                                       | order-created            | pass	 |
|                  | Get              | api/v1/orders                                       |                          |		 |
|                  | Get              | api/v1/orders/{orderId}                             |                          |		 |
|                  | Update           | api/v1/orders/{orderId}                             |                          |		 |
|                  | Delete           | api/v1/orders/{orderId}                             |                          |		 |
| OrderDetails	   | Create           | 													|                          |		 |
|                  | Get              | 													|                          | 		 |
|                  | Get              | 													|                          | 		 |
|                  | Update           | 													|                          | 		 |
|                  | Delete           | 													|                          | 		 |
|Consume Ticket    | Created          | 													| ticket-created           | pass    |
|                  | Deleted          | 													| ticket-deleted           | pass    |
|Consume Customer  | Created          | 													| customer-created         | pass	 |
|                  | Deleted          | 													| customer-deleted         | pass    |
|Consume Order     |                  | 													| stock-reversed           | pass    |
|				   |                  | 													| stock-reversed-failed    | pass	 |
|				   |                  | 													| order-validated          | pass	 |
|				   |                  | 													| payment-processed        | 	     |
|				   |                  | 													| payment-processed-failed | 	     |

## Service Payment

| Entity           | Action           | Url                                                 | Event - Queue            | Status   |
|------------------|------------------|-----------------------------------------------------|--------------------------|----------|
| Invoice	       | Create           | api/v1/invoices                                     |                          | pass	  |
|                  | Get              | api/v1/invoices                                     |                          |		  |
|                  | Get              | api/v1/invoices/{invoiceId}                         |                          |		  |
|                  | Update           | api/v1/invoices/{invoiceId}/payment                 | payment-processed        | pass	  |
|                  | Update           | api/v1/invoices/{invoiceId}/cancel                  | payment-processed-failed | pass	  |
|                  | Delete           | api/v1/invoices/{invoiceId}                         |                          |		  |
| Consume Customer | Created          | 													| customer-created         | pass	  |
|                  | Deleted          | 													| customer-deleted         | pass     |
| Consume Order    | Created          | 													| order-validated          | pass     |


