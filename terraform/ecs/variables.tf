variable "tg_arn" {
    description = "ARN of target group"
    type = string
}

variable "image_tag" {
  description = "The tag of the container image to deploy"
  type        = string
  default     = "PLACEHOLDER_VALIDATE_ONLY"
}

