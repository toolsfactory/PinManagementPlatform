# Sample Service for Pinmanagement

A dummy service for pin management to try out standard concepts and principles of microservice design in .net core.

## Implemented support services
All services are exposed on different ports of localhost via http

### Database Administration
Adminer as simple DB Administration tool is running on port 5002. 
Username and password for the DB need to be configured in the docker-compose.yaml but also some source code files (tbc).

http://localhost:5002/

### Healthcheck UI
All services implement simple health check functionalities as recommended for any microservice.
A small UI is present via port 5000.

http://localhost:5000/healthchecks-ui

### Redis
A single node redis instance without any special configuration is accessible via port 6379

### mariadb
As MySQL comaptibe database backend, mariadb is running and accessibel via port 3306.
For details on configuration and volumes setup please check the docher compose yaml.

### excpetionless
To show how central log collection could look like, an instance of Exceptionless is running in a separate container.
The execptionless UI can be reached via port 5001

http://localhost:5001/

## The API Services

All api services expose a swagger ui via their dedicated port on that url: http://localhost:XX/swagger.index.html

### ClientAPI
on port 80 the client api with its subfunctions can be reached. 
Most of the functions are implemented and do something "useful".
http://localhost:80/swagger.index.html

### AdminAPI
on port 89 an admin api with its subfunctions can be reached. 
This api should help to test and verify the other parts of the solution and for example allow to generate random pins, clear the cache and so on.
http://localhost:89/swagger.index.html

### ProvisioningAPI
Future API exposed to BSS/OSS systems. not yet functional.
http://localhost:81/swagger.index.html


# Other apsects

## Authentication
At the moment, authentication is disabled. But code to add JWT base authentication is already in the solution and could easily be activeated to also show that part


