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
  image_tag = var.image_tag
  ecs_private_subnet_one = module.network.ecs_private_subnet_one_id
  ecs_private_subnet_two = module.network.ecs_private_subnet_two_id
  ecs_task_sg = module.network.ecs_task_sg_id
}