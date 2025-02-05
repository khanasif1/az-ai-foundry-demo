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
        "gpt-apikey": "EwAfYEWgZWfzJwZjWgAcRnirKcEqZgJ6XjEUGeROrklnVMum3HiBJQQJ99ALACYeBjFXJ3w3AAAAACOGXzsT",
        "sasToken": "sp=racwdl&st=2025-02-05T02:32:16Z&se=2025-02-11T10:32:16Z&skoid=2d4e77c9-e388-4f19-a999-e74353694e0d&sktid=16b3c013-d300-468d-ac64-7eda0820b6d3&skt=2025-02-05T02:32:16Z&ske=2025-02-11T10:32:16Z&sks=b&skv=2022-11-02&spr=https&sv=2022-11-02&sr=c&sig=rYfPD2QuVatIPRyuYQ89LZKMFA1UNoqbd06ETKlEH6A%3D",
        "vision-key": "FDQGHx2nrS9VvYVpMFRkZBQPsiewAGNF4NLRsX9IkCxXLAozIoRuJQQJ99ALACYeBjFXJ3w3AAAFACOGQL8n",
        "vision-endpoint": "https://demo-ocr-computer-vision.cognitiveservices.azure.com/"
    },
    "AllowedHosts": "*"

}
```