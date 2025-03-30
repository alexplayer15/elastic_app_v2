output "ecs_priv_sub_id" {
  value = aws_subnet.ecs_private_subnet.id
}

output "alb_sg_id" {
    value aws_security_group.alb_sg.id
}

