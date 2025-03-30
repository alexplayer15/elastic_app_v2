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

module "load_balancer" {
  source = "./load_balancer"
  ecs_priv_sub_id = module.network.ecs_priv_sub_id
  alb_sg_id = module.network.alb_sg_id
}