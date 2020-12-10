using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace ClientTests
{
    public class ConsumerMyApiPact : IDisposable
    {
        private IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; }

        private static int MockServerPort => 9222;
        public static string MockProviderServiceBaseUri => $"http://localhost:{MockServerPort}";

        public ConsumerMyApiPact()
        {
            PactBuilder = new PactBuilder();

            PactBuilder
                .ServiceConsumer("Weather API Consumer")
                .HasPactWith("Weather API Provider");

            MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }
    }
}