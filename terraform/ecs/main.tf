# resource "aws_ecs_service" "WebApp" {
#   name            = "elastic_app"
#   cluster         = aws_ecs_cluster.foo.id
#   task_definition = aws_ecs_task_definition.mongo.arn
#   desired_count   = 1
#   iam_role        = aws_iam_role.foo.arn
#   depends_on      = [aws_iam_role_policy.foo]

#   ordered_placement_strategy {
#     type  = "binpack"
#     field = "cpu"
#   }

#   load_balancer {
#     target_group_arn = aws_lb_target_group.foo.arn
#     container_name   = "elastic_app"
#     container_port   = 8080
#   }

#   placement_constraints {
#     type       = "memberOf"
#     expression = "attribute:ecs.availability-zone in [eu-west-2a, eu-west-2b, eu-west-2c]"
#   }
# }