# Azure Batch Client Sample App

This sample app is from this [MS Learn module.](https://docs.microsoft.com/en-us/learn/modules/create-an-app-to-run-parallel-compute-jobs-in-azure-batch/5-exercise-access-your-batch-account-using-the-dotnet-client-library)

## Deploy Azure Resources

Run the following Azure CLI command to create the Azure Batch Account and related Azure resources:

```powershell
az deployment sub create `
    -l {resource group location} `
    -f .\deploy\main.bicep `
    -p .\deploy\parameters.dev.json
```

## Create the FFMPEG Application Package

Run the following Azure CLI command to create the FFMPEG application package:

```powershell
 az batch application package create `
    --application-name ffmpeg `
    --n {batch account name} `
    --package-file '.\BatchApplicationPackages\ffmpeg-3.4-win64-static (1).zip' `
    -g {resource group name} `
    --version-name 3.4
```

The batch account and resource group names can be found in the Azure Resource deployment output.

## Modify the App Settings File

Open appsettings.json and fill in values for all missing values. The values can be found in the Azure Resource deployment output.

## Run the Batch Client Application

Run the AzBatchClient project. The application uploads the videos in the `InputFiles` folder to the `input` blob container.
