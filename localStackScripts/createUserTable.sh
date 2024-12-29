#!/bin/sh
set -e 
echo "Creating User Data table"
sleep 2  
awslocal dynamodb create-table --region eu-west-1 --cli-input-json file:///etc/localstack/init/ready.d/dynamoDbSchemas/UserData.json
echo "table created"