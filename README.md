# Tax Calculator

## Build and run

`docker-compose up -d --build --remove-orphans`

OR

Run through VS/Rider, TaxCalculator profile

## Test

Visit `http://localhost:5100/swagger` or open `test.http` file in VS Code and execute requests using the "REST Client" extension.

## Run integration tests

`docker build . -t tax_calculator`

Run integration tests through VS/Rider

## Shut down

`docker-compose down`
