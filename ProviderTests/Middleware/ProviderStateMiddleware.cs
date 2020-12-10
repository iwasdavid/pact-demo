using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Newtonsoft.Json;

namespace ProviderTests.Middleware
{
    public class ProviderStateMiddleware
    {
        private const string ConsumerName = "Weather API Consumer";
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "WeatherForecast api returns 2 forecast",
                    SetupDbState
                }
            };
        }

        private void SetupDbState()
        {
            // If we were talking to a test db, setup test data here
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value == "/provider-states")
            {
                await HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(string.Empty);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int) HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() && context.Request.Body != null)
            {
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
                var jsonRequestBody = await reader.ReadToEndAsync();

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !string.IsNullOrEmpty(providerState.State) && providerState.Consumer == ConsumerName)
                    _providerStates[providerState.State].Invoke();
            }
        }
    }
}