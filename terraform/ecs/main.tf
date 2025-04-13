resource "aws_ecs_service" "elastic_app_v2_service" {
  name            = "elastic-app-v2"
  cluster         = aws_ecs_cluster.elastic_app_v2_cluster.id
  task_definition = aws_ecs_task_definition.elastic_app_v2_task_definition.arn
  desired_count   = 1
  iam_role        = aws_iam_role.foo.arn
  depends_on      = [aws_iam_role_policy.foo]

  ordered_placement_strategy {
    type  = "binpack"
    field = "cpu"
  }

  load_balancer {
    target_group_arn = var.tg_arn
    container_name   = "elastic_app_v2"
    container_port   = 8080
  }

  placement_constraints {
    type       = "memberOf"
    expression = "attribute:ecs.availability-zone in [eu-west-2a, eu-west-2b, eu-west-2c]"
  }
}

resource "aws_ecs_cluster" "elastic_app_v2_cluster" {
  name = "elastic-app-v2-cluster"

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
}

resource "aws_ecs_task_definition" "elastic_app_v2_task_definition" {
  family = "service"
  container_definitions = jsonencode([
    {
      name      = "elastic-app-v2"
      image     = "174558992457.dkr.ecr.eu-west-2.amazonaws.com/elastic_app_v2:latest"
      cpu       = 10
      memory    = 512
      essential = true
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
        }
      ]
    }
  ])

  volume {
    name      = "service-storage"
    host_path = "/ecs/service-storage"
  }

  placement_constraints {
    type       = "memberOf"
    expression = "attribute:ecs.availability-zone in [eu-west-2a, eu-west-2b]"
  }
}