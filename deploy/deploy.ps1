# this assumes some resources exist (acr, container environment)

$RESOURCE_GROUP="rg-kennissessie-nov2022"
$ENVIRONMENT="managedEnvironment-rgkennissessien-92be"
$ACR_NAME="acrkennissessie"
$LOCATION="westeurope"
$STORAGE_ACCOUNT_NAME="sadaprstatenov2022"

az login

# create storage
az storage account create `
  --name $STORAGE_ACCOUNT_NAME `
  --resource-group $RESOURCE_GROUP `
  --location "$LOCATION" `
  --sku Standard_RAGRS `
  --kind StorageV2

cd ..

# deploy dapr state component using blob storage
az containerapp env dapr-component set `
    --name $ENVIRONMENT --resource-group $RESOURCE_GROUP `
    --dapr-component-name statestore `
    --yaml ./deploy/statestore.yaml

# API


az acr build --registry $ACR_NAME --image counter-api:latest . -f .\CounterApi\Dockerfile

az containerapp create `
  --name 'counter-api' `
  --resource-group $RESOURCE_GROUP `
  --environment $ENVIRONMENT `
  --image "$ACR_NAME.azurecr.io/counter-api:latest"  `
  --target-port 80 `
  --ingress 'internal' `
  --registry-server "$ACR_NAME.azurecr.io" `
  --min-replicas 1 `
  --max-replicas 1 `
  --enable-dapr `
  --dapr-app-port 80 

az containerapp update -n counter-api -g $RESOURCE_GROUP --image $ACR_NAME.azurecr.io/counter-api:latest


# Error: No credential was provided to access Azure Container Registry. Trying to look up credentials...
# Failed to retrieve credentials for container registry acrkennissessie. Please provide the registry username and password
# Fix: enable admin credentials


# frontend
az acr build --registry $ACR_NAME --image counter-frontend:latest . -f .\CounterFrontend\Dockerfile

az containerapp create `
  --name 'counter-frontend' `
  --resource-group $RESOURCE_GROUP `
  --environment $ENVIRONMENT `
  --image "$ACR_NAME.azurecr.io/counter-frontend:latest"  `
  --target-port 80 `
  --ingress 'external' `
  --registry-server "$ACR_NAME.azurecr.io" `
  --min-replicas 1 `
  --max-replicas 1 `
  --enable-dapr `
  --dapr-app-port 80 


  # Update is not yet working?
  az containerapp update -n counter-frontend -g $RESOURCE_GROUP --image $ACR_NAME.azurecr.io/counter-frontend:latest
