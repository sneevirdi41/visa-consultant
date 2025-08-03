#!/bin/bash

echo "Starting Railway deployment for Visa Consultant App..."

# Wait for database to be ready
echo "Waiting for database to be ready..."
sleep 10

# Run database migrations
echo "Running database migrations..."
dotnet ef database update --verbose

# Check if migrations were successful
if [ $? -eq 0 ]; then
    echo "Database migrations completed successfully!"
else
    echo "Database migrations failed. Retrying..."
    sleep 5
    dotnet ef database update --verbose
fi

# Start the application
echo "Starting the application..."
dotnet visa_consulatant.dll 