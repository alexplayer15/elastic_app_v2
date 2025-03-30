output "alb_pub_sub_one_id" {
    value = aws_subnet.alb_public_subnet_one.id
}

output "alb_pub_sub_two_id" {
    value = aws_subnet.alb_public_subnet_two.id
}

output "alb_sg_id" {
    value = aws_security_group.alb_sg.id
}

