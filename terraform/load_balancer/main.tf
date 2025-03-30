resource "aws_lb" "main_alb" {
  name = "elastic-app-v2-alb"
  internal           = false
  load_balancer_type = "application"
  subnets = [var.ecs_priv_sub_one_id, var.ecs_priv_sub_two_id]
  security_groups    = [var.alb_sg_id]

  tags = {
    Name = "elastic_app_v2_alb"
  }
  enable_deletion_protection = false 

  depends_on = [var.ecs_priv_sub_one_id, var.ecs_priv_sub_two_id]
}