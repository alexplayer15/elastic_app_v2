resource "aws_lb" "main_alb" {
  name = "elastic-app-v2-alb"
  internal           = false
  load_balancer_type = "application"
  subnets = [var.alb_pub_sub_one_id, var.alb_pub_sub_two_id]
  security_groups    = [var.alb_sg_id]

  tags = {
    Name = "elastic_app_v2_alb"
  }
  enable_deletion_protection = false 

  depends_on = [var.alb_pub_sub_one_id, var.alb_pub_sub_two_id]
}

resource "aws_lb_target_group" "elastic_app_v2_tg" {
  name        = "elastic-app-v2-tg"
  target_type = "ip"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = var.main_vpc_id

  health_check {
    path                = "/health"
    protocol            = "HTTP"
    matcher             = "200"   
    interval            = 30        
    timeout             = 5         
    healthy_threshold   = 2       
    unhealthy_threshold = 2         
  }
}


resource "aws_lb_listener" "main_alb_http_listener" {
  load_balancer_arn = aws_lb.main_alb.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.elastic_app_v2_tg.arn
  }
}

//TO DO - ADD CERT FOR SSL 
# resource "aws_lb_listener" "main_alb_listener" {
#   load_balancer_arn = aws_lb.main_alb.arn
#   port              = "443"
#   protocol          = "HTTPS"
#   ssl_policy        = "ELBSecurityPolicy-2016-08"
#   certificate_arn   = "arn:aws:iam::187416307283:server-certificate/test_cert_rab3wuqwgja25ct3n4jdj2tzu4"

#   default_action {
#     type             = "forward"
#     target_group_arn = aws_lb_target_group.elastic_app_v2_tg.arn
#   }
# }

