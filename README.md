# Azure Batch Client Sample App

This sample app is from this [MS Learn module.](https://docs.microsoft.com/en-us/learn/modules/create-an-app-to-run-parallel-compute-jobs-in-azure-batch/5-exercise-access-your-batch-account-using-the-dotnet-client-library)

## App Settings
Run the following Azure CLI command to get the Batch account endpoint:
```bash
az batch account list --query "[].accountEndpoint"
```

Run the following Azure CLI command to get the Batch account name:
```bash
az batch account list --query "[].name"
```

Run the following Azure CLI command to get the Batch account key:
```bash
az batch account keys list --name <<batch account name>> --query "primary" --resource-group <<resource group name>>
```
