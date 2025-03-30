variable "allow_all_CIDR" {
    type = string 
    description = "CIDR allowing traffic on all IPv4 addresses"
    default = "0.0.0.0/0"
}