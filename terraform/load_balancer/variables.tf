variable "alb_pub_sub_one_id" {
    type = string 
    description = "Public subnet for ALB"
}

variable "alb_pub_sub_two_id" {
    type = string 
    description = "Public subnet for ALB"
}

variable "alb_sg_id"{
    type = string 
    description = "ALB security group ID"
}

variable "main_vpc_id" {
    type = string 
    description = "main vpc id"
}