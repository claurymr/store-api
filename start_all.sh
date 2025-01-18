#!/bin/bash

# Move this file to the root directory of all projects
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

# List of services and their directories
SERVICES=(
  "inventory-auth-service"
  "product-service"
  "inventory-service"
  "store-api"
)

# Function to start a service using docker-compose
start_service() {
  local service_dir=$1
  echo "Starting Docker Compose for $service_dir..."
  cd "$PROJECT_DIR/$service_dir" || exit 1
  docker-compose -f docker-compose.yml up -d --build
}

# Export the function and variables for parallel execution
export -f start_service
export PROJECT_DIR

# Start all services in parallel
parallel -j 0 start_service ::: "${SERVICES[@]}"

# Show the status of all running containers
echo "All services are up and running. Here is the status of your containers:"
docker ps

# End of script
echo "Docker setup complete!"
