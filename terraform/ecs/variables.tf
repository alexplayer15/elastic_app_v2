variable "tg_arn" {
    description = "ARN of target group"
    type = string
}

variable "image_tag" {
  description = "The tag of the container image to deploy"
  type        = string
  default     = "PLACEHOLDER_VALIDATE_ONLY"
}

variable "ecs_private_subnet_one" {
  description = "One of the private subnets the ECS Fargate tasks will be hosted in"
  type        = string
}

variable "ecs_private_subnet_two" {
  description = "One of the private subnets the ECS Fargate tasks will be hosted in"
  type        = string
}

variable "ecs_task_sg" {
    description = "Security group for ECS tasks"
    type        = string 
}
