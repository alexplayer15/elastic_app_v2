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
  alb_pub_sub_one_id = module.network.alb_pub_sub_one_id
  alb_pub_sub_two_id = module.network.alb_pub_sub_two_id
  alb_sg_id = module.network.alb_sg_id
  main_vpc_id = module.network.main_vpc_id
}

module "ecs" {
  source = "./ecs"
  tg_arn = module.load_balancer.tg_arn
}