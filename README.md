# azure-serverless-api

Code companion for the "Building Serverless APIs on Microsoft Azure with .NET 6" presentation.

## Prerequisites

* [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
* [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools) version 4.x.
* [Docker Desktop](https://docs.docker.com/desktop/)

## Project Details

* InProcessStatic: in-process function, HTTP Trigger, .NET 6, Blob and Queue bindings, static implementation.
* IsolatedProcessIoC: isolated process function, HTTP Trigger, .NET 6, Blob and Queue bindings, Open API support, Inversion of Control.
* IntegrationTests: simple integration tests for both functions

## Running Locally

```bash
docker-compose up -d
```

```bash
cd src/InProcessStatic

func start
```

```bash
cd src/IsolatedProcessIoC

func start
```

```bash
cd tests/IntegrationTests

dotnet test
```

Note that the InProcessStatic and IsolatedProcessIoC projects use the same local port so you can not run them simultaneously with the current configuration. 
