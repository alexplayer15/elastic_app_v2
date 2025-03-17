#!/bin/sh
set -e 
echo "Populating User Data table"
sleep 2 

USER_TABLE_NAME="UserData"

awslocal dynamodb put-item \
	--table-name "$USER_TABLE_NAME" \
	--item file:///etc/localstack/init/ready.d/UserData/OneUser.json

echo "User Data table populated"

echo "Populating Token Data table"
sleep 2 

TOKEN_TABLE_NAME="TokenData"

awslocal dynamodb put-item \
	--table-name "$TOKEN_TABLE_NAME" \
	--item file:///etc/localstack/init/ready.d/OneToken.json

echo "Token Data table populated"