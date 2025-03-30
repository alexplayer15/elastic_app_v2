terraform {
  backend "s3" {
    bucket         = "elastic-app-v2-terraform-state"  
    key            = "terraform.tfstate"  
    region         = "eu-west-2"                
    encrypt        = true   
    dynamodb_table = "elastic-app-v2-state-locking"                 
  }
}

module "network" {
  source = "./network"
}