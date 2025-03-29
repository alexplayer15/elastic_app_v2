terraform {
  backend "s3" {
    bucket         = "elastic-app-v2-terraform-state"  
    key            = "terraform.tfstate"  
    region         = "eu-west-2"                
    encrypt        = true                   
  }
}

module "network" {
  source = "./network"
}