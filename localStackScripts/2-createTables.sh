#!/bin/sh
set -e 
echo "Creating User Data table"
sleep 2  
awslocal dynamodb create-table --region eu-west-2 --cli-input-json file:///etc/localstack/init/ready.d/dynamoDbSchemas/UserDataTable.json
echo "User data table created"

echo "Creating Token Data table"
sleep 2
awslocal dynamodb create-table --region eu-west-2 --cli-input-json file:///etc/localstack/init/ready.d/dynamoDbSchemas/TokenDataTable.json