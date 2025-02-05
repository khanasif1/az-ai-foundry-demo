# Setup Dotnet Solution

## Build and Run

- Add a appsettings.json file at ~\az-ai-foundry-demo\sk\dotnet\sk.dotnet.poc\sk.api\ :
```
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AppSettings": {
        "gpt-apikey": "",
        "sasToken": "",
        "vision-key": "",
        "vision-endpoint": ""
    },
    "AllowedHosts": "*"

}
```