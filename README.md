# .NET 5 Pact Demo

Very basic .NET solution showing the concepts involved in consumer driven contract testing with [Pact](https://pact.io/).

### Projects in Solution

#### Client App (Consumer)
C# console app that call the Weather API using a HTTP GET request

#### Client Tests
C# Unit tests for the Client App (Consumer). These tests generate the contracts that the Provider has to ensure that is met.

#### WebApplicationAPI (Provider)
C# Web API that just returns n amount of weather forecasts as requested by the client

#### Provider Tests
C# unit tests for ensuring that the the provider is keeping to the contracts generated by the consumer. **The WebApplicationAPI has to be running for these tests to pass**.
