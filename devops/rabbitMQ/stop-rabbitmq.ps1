$projectName=$args[0]
$ambiente=$args[1]
write-host "Parametros defaults, Sintaxe customizada: run-rabbitmq.ps1 projectName local|dev|tst|hml|prd" 
write-host "Parametros defaults, default: run-rabbitmq.ps1 projectName local" 

if ($projectName -eq $null) {
    $projectName = "projectName"
}

if ($ambiente -eq $null) {
    $ambiente = "local"
}

$dockerProjectName=$projectName + '_rabbitmq_' + $ambiente
$dockerFileName='docker-comp-rabbitmq-' + $ambiente + '.yml'

docker-compose -p $dockerProjectName -f ./devops/rabbitMQ/$dockerFileName down