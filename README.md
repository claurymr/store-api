# Store Service

> [!IMPORTANT]
> Further documnentation including Architectural and Flow Diagram can be found in the [Wiki Overview](https://github.com/claurymr/store-api/wiki/Overview)

> [!IMPORTANT] 
> This solution is part of the Inventory Management System and should be run together with the rest of the services. The whole system is composed of the following components:
> - [API Gateway](https://github.com/claurymr/store-api) <--- Complete setup instructions can be found here.
> - [Inventory Auth Service](https://github.com/claurymr/inventory-auth-service)
> - [Product Service](https://github.com/claurymr/product-service) 
> - [Inventory Service](https://github.com/claurymr/inventory-service) 

> **Check out official Wiki for further documentation**

## Overview
The Store API is an API Gateway designed to handle routing to other apis, and services/microservices. It provides common endpoints for all requests and re-routes them to their respective api or service.

This solution uses Docker for containerization, making it easy to run on macOS, Linux, and Windows.

## Development Approach Decisions

### Api Gateway with Ocelot 
I implemented a modular approach and opted to keep everything in one project since this is a simple setup for Ocelot Api Gateway.

Using Ocelot, I configured the API Gateway to handle endpoint redirection to the respective services within the Inventory Management System. Ocelot allows for defining routing rules in a configuration file, which maps incoming requests to the appropriate downstream services. Ocelot enforces authentication and authorization by integrating with the JWT Auth setup. Each incoming request is validated for a valid JWT token before being routed to the respective service. This ensures that only authenticated and authorized requests are processed, maintaining the security and integrity of the system.

### JWT Auth
To handle authentication and authorization, I implemented a simple JWT Auth by setting up predetermined credentials that don't need to be saved to a database. This decision was taken to simplify the process of authentication and authorization while prioritizing other functionalities, as registration and login were not required for the nature of this project. This approach allows for secure access control without the overhead of managing user credentials in a database.

### Env and Script Files
.env and .sh files were generate to facilitate orchestration of whole system.

## Build Whole Inventory System (Using bash or terminal in macOs/Linux)

Ensure Prerequisites are met [Check the wiki documentation]()

1. Navigate to desired root directoru
```bash
cd /path/to/your/root/directory
```
2. Clone all repositories indicate at the beginning of the README file.
```bash
git clone https://github.com/claurymr/store-api.git
git clone https://github.com/claurymr/inventory-auth-service.git
git clone https://github.com/claurymr/product-service.git
git clone https://github.com/claurymr/inventory-service.git
```
3. Move .env and .sh files from `store-api` folder to root folder (make hidden files visible)
4. Open terminal in root directory or navigate to it
```bash
cd /path/to/your/root/directory
```
5. Use bash to allow the running of scripts.
```bash
chmod +x start_all.sh
chmod +x stop_all.sh
```
6. Run Docker for all
```bash
./start_all.sh
```
7. To stop docker for all
```bash
./stop_all.sh
```
8. Import `.postman_collection.json` file to postman located in `/src` of root of repo, for testing endpoints once all services are up and running.
9. To change valid user credentials to admin priviledges, change the following environment variables in .env and run environment vars setup.
```
AUTH__USERNAME=admin 
AUTH__PASSWORD=admin
AUTH__ROLE=admin
```
9. To change valid user credentials to user restrictions, change the following environment variables in .env and run environment vars setup.
```
AUTH__USERNAME=user 
AUTH__PASSWORD=restricted
AUTH__ROLE=user
```
