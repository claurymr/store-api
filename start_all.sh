#!/bin/bash

# Move this file to root directory of all projects
# Set the working directory to the root project folder (where this script is located)
PROJECT_DIR=$(pwd)

# Load environment variables from the .env file
if [ -f "$PROJECT_DIR/.env" ]; then
  export $(grep -v '^#' "$PROJECT_DIR/.env" | xargs)
else
  echo ".env file not found! Please create one with the necessary environment variables."
  exit 1
fi

# Create the shared Docker network if it doesn't already exist
docker network inspect shared-network > /dev/null 2>&1
if [ $? -ne 0 ]; then
  echo "Creating shared Docker network: shared-network"
  docker network create shared-network
else
  echo "Shared Docker network already exists: shared-network"
fi

# Run Docker Compose for ProductService
echo "Running Docker Compose for Microservice ProductService..."
cd "$PROJECT_DIR/product-service"  # Change to the Service1 directory
docker-compose -f docker-compose.yml up -d --build

# Run Docker Compose for InventoryService
echo "Running Docker Compose for Microservice InventoryService..."
cd "$PROJECT_DIR/inventory-service"  # Change to the Service2 directory
docker-compose -f docker-compose.yml up -d --build

# Run Docker Compose for the StoreApi (API Gateway)
echo "Running Docker Compose for StoreApi..."
cd "$PROJECT_DIR/store-api"  # Change to the StoreApi directory
docker-compose -f docker-compose.yml up -d --build

# Show the status of all running containers
echo "All services are up and running. Here is the status of your containers:"
docker ps

# End of script
echo "Docker setup complete!"