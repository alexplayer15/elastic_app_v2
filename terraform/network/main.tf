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
    cidr_block = "0.0..0/0"
    gateway_id = aws_internet_gateway.main_vpc_igw.id
  }
}

resource "aws_route_table_association" "alb_pub_sub_assocation" {
  subnet_id      = aws_subnet.alb_public_subnet.id
  route_table_id = aws_route_table.main_pub_sub_route_table.id
}