# Prepare local Dapr
1. Install dapr cli: https://docs.dapr.io/getting-started/install-dapr-cli/
2. Initialize local dapr env: https://docs.dapr.io/getting-started/install-dapr-cli/

This should initialize dapr with some default components

# Check Dapr dashboard:
dapr dashboard


# Local testing of CounterApi
cd ./CounterApi
dapr run --app-id counter-api --app-port 5211 dotnet run


## Invoke Dapr using local cli
dapr invoke --app-id counter-api --method count --verb GET

dapr invoke --app-id counter-api --method count/increment


# Local testing of Frontend
cd ./CounterFrontend

dapr run --app-id frontend --app-port 5222 dotnet run

browse to: http://localhost:5222/

# Deployment
See ./deploy/


# Dapr limitations on Container apps
https://learn.microsoft.com/en-us/azure/container-apps/dapr-overview?tabs=bicep1%2Cyaml#unsupported-dapr-capabilities