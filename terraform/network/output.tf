output "ecs_priv_sub_one_id" {
    value = aws_subnet.ecs_private_subnet_one.id
}

output "ecs_priv_sub_two_id" {
    value = aws_subnet.ecs_private_subnet_two.id
}

output "alb_sg_id" {
    value = aws_security_group.alb_sg.id
}

