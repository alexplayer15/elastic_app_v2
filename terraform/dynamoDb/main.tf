# resource "aws_dynamodb_table" "users_table" {
#   name           = "UserData"              
#   billing_mode   = "PAY_PER_REQUEST" 
#   hash_key       = "id"                 

#   attribute {
#     name = "id"                          
#     type = "S"                           
#   }

#   ttl {
#     attribute_name = "TimeToExist"
#     enabled        = true
#   }

#   tags = {
#     Name        = "UserData"
#     Environment = "production"
#   }
# }