
PROJECT = MiniPricingPlatform.API

.PHONY: build run test clean

build:
	dotnet build $(PROJECT)

run:
	dotnet run --project $(PROJECT)/$(PROJECT).csproj

test:
	dotnet test

clean:
	dotnet clean

dup:
	docker-compose up --build -d