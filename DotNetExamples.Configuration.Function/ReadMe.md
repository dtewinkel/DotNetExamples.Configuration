# Run locally

The project should run locally based on the settings in `Properties/launchSettings.json`. 
If this is not the case, then create a file named `local.settings.json` and set contents to:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  }
}
```