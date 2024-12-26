#!/bin/sh
set -e #to exit on first error
echo "Running create-table.sh..."
sleep 2  # Wait for 2 seconds
awslocal dynamodb create-table --region eu-west-1 --cli-input-json file:///etc/localstack/init/ready.d/dynamoDbSchemas/userTableSchema.json
echo "table created"