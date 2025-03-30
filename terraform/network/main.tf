//Public networks

resource "aws_vpc" "main_vpc" {
  cidr_block       = "10.0.0.0/16"

  tags = {
    Name = "main_vpc"
  }
}

resource "aws_subnet" "alb_public_subnet" {
  vpc_id     = aws_vpc.main_vpc.id
  cidr_block = "10.0.1.0/24"

  tags = {
    Name = "ALB_pub_sub"
  }
}

resource "aws_internet_gateway" "main_vpc_igw" {
  vpc_id = aws_vpc.main_vpc.id

  tags = {
    Name = "main"
  }
}

resource "aws_route_table" "main_pub_sub_route_table" {
  vpc_id = aws_vpc.main_vpc.id

  route {
    cidr_block = var.allow_all_CIDR
    gateway_id = aws_internet_gateway.main_vpc_igw.id
  }
}

resource "aws_route_table_association" "alb_pub_sub_assocation" {
  subnet_id      = aws_subnet.alb_public_subnet.id
  route_table_id = aws_route_table.main_pub_sub_route_table.id
}

//Private networks

resource "aws_subnet" "ecs_private_subnet" {
  vpc_id     = aws_vpc.main_vpc.id
  cidr_block = "10.0.2.0/24"

  tags = {
    Name = "ECS_priv_sub"
  }
}

resource "aws_route_table" "main_priv_sub_route_table" {
  vpc_id = aws_vpc.main_vpc.id
}

resource "aws_route_table_association" "ecs_priv_sub_association" {
  subnet_id      = aws_subnet.ecs_private_subnet.id
  route_table_id = aws_route_table.main_priv_sub_route_table.id
}

resource "aws_vpc_endpoint" "dynamoDb_vpc_endpoint" {
  vpc_id       = aws_vpc.main_vpc.id
  service_name = "com.amazonaws.eu-west-2.dynamodb"
  vpc_endpoint_type = "Gateway"
  route_table_ids = [aws_route_table.main_priv_sub_route_table.id]

  tags = {
    Name = "DynamoDB VPC Endpoint"
  }
}

resource "aws_vpc_endpoint" "ecr_interface_vpc_endpoint" {
  vpc_id            = aws_vpc.main_vpc.id
  service_name      = "com.amazonaws.eu-west-2.ecr"
  vpc_endpoint_type = "Interface"

  security_group_ids = [
    aws_security_group.ecr_vpc_endpoint_sg.id
  ]

  subnet_ids = [aws_subnet.ecs_private_subnet.id]

  private_dns_enabled = true

  tags = {
    Name = "ECR VPC Endpoint"
  }
}

//Security groups
resource "aws_security_group" "alb_sg" {
  vpc_id = aws_vpc.main_vpc.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = [var.allow_all_CIDR]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = [var.allow_all_CIDR]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"  
    cidr_blocks = [var.allow_all_CIDR]
  }

  tags = {
    Name = "ALB Security Group"
  }
}

resource "aws_security_group" "ecs_task_sg" {
  vpc_id = aws_vpc.main_vpc.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    security_groups = [aws_security_group.alb_sg.id]
  }

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    security_groups = [aws_security_group.alb_sg.id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"  
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "Priv Sub security group"
  }
}

resource "aws_security_group" "ecr_vpc_endpoint_sg" {
  vpc_id = aws_vpc.main_vpc.id

  ingress {
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    security_groups = [aws_security_group.ecs_task_sg.id] 
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"  
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "ECR VPC Endpoint Security Group"
  }
}
