#!/bin/sh
set -e 
echo "Populating User Data table"
sleep 2 

TABLE_NAME="UserData"

awslocal dynamodb put-item \
	--table-name "$TABLE_NAME" \
	--item file:///etc/localstack/init/ready.d/UserData/OneUser.json

echo "table populated"