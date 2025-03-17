#!/bin/sh
set -e 
echo "Creating dummy email credentials"

. /run/secrets/email_credentials

#sanitise line endings
EMAIL_USERNAME=$(echo "$EMAIL_USERNAME" | tr -d '\r')
EMAIL_PASSWORD=$(echo "$EMAIL_PASSWORD" | tr -d '\r')

echo "Username: $EMAIL_USERNAME"
echo "Password: $EMAIL_PASSWORD"

awslocal secretsmanager create-secret \
	--name SESEmailCredentials \
	--secret-string "{\"Username\": \"$EMAIL_USERNAME\", \"Password\": \"$EMAIL_PASSWORD\"}" \
	--region eu-west-2

echo "dummy credentials successfully created"