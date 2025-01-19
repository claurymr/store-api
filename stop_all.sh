#!/bin/bash

# Set the working directory to the root project folder (where this script is located)
PROJECT_DIR=$(pwd)

# List of services and their directories
SERVICES=(
  "inventory-auth-service"
  "product-service"
  "inventory-service"
  "store-api"
)

# Function to stop a service using docker-compose
stop_service() {
  local service_dir=$1
  echo "Stopping Docker Compose for $service_dir..."
  cd "$PROJECT_DIR/$service_dir" || exit 1
  docker-compose -f docker-compose.yml down
}

# Export the function and variables for parallel execution
export -f stop_service
export PROJECT_DIR

# Stop all services in parallel
echo "Stopping all services..."
parallel -j 0 stop_service ::: "${SERVICES[@]}"

# Remove the shared Docker network if it exists
NETWORK_NAME="shared-network"
if docker network inspect $NETWORK_NAME > /dev/null 2>&1; then
  echo "Removing shared Docker network: $NETWORK_NAME"
  docker network rm $NETWORK_NAME
else
  echo "Shared Docker network does not exist: $NETWORK_NAME"
fi

# Optionally clean up unused Docker resources
read -p "Do you want to remove unused Docker volumes, networks, and images? (y/N): " CLEANUP
if [[ "$CLEANUP" =~ ^[Yy]$ ]]; then
  echo "Cleaning up unused Docker resources..."
  docker system prune -f --volumes
else
  echo "Skipping Docker cleanup."
fi

# Show the status of remaining containers (if any)
echo "Here is the status of your Docker containers:"
docker ps -a

# End of script
echo "Docker teardown complete!"
