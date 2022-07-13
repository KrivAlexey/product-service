# product-service
CRUD for products 
# Getting Started
## Prerequisites
[.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)  
docker 

## Installing
* clone the repo:
    ```
    git https://github.com/KrivAlexey/product-service.git
    cd product-service
    ```
* build and run  
`docker compose up -d`
* run tests  
With running db service container from docker compose file, execute:  
`dotnet test`

## Service documentation
Swagger available at the endpoint https://localhost:7286/index.html when running with the `Development` environment.