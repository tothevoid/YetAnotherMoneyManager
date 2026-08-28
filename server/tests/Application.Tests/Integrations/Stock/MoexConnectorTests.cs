using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Audex.Application.DTO.Securities;
using Audex.Application.Integrations.Stock.Moex;
using Audex.Infrastructure.Constants;
using Xunit;

namespace Audex.Application.Tests.Integrations.Stock
{
    public class MoexConnectorTests
    {
        [Fact]
        public async Task GetCandles_ByTicker_ShouldParseCandlesCorrectly()
        {
            // Arrange
            var jsonResponse = @"{
                ""candles"": {
                    ""columns"": [""open"", ""close"", ""high"", ""low"", ""value"", ""volume"", ""begin"", ""end""],
                    ""data"": [
                        [7825.0, 7830.0, 7840.0, 7810.0, 1523420.5, 195.0, ""2026-08-02 00:00:00"", ""2026-08-02 23:59:59""]
                    ]
                }
            }";

            var handler = new MockHttpMessageHandler(jsonResponse);
            var httpClient = new HttpClient(handler);
            var factory = new StubHttpClientFactory(httpClient);

            var connector = new MoexConnector(factory);
            var from = new DateOnly(2026, 8, 2);
            var till = new DateOnly(2026, 8, 9);

            var security = new SecurityDto { Ticker = "SBER" };

            // Act
            var result = (await connector.GetCandlesAsync(security, from, till, 24)).ToList();

            // Assert
            Assert.Single(result);
            var candle = result[0];
            Assert.Equal(7825.0m, candle.Open);
            Assert.Equal(7830.0m, candle.Close);
            Assert.Equal(7840.0m, candle.High);
            Assert.Equal(7810.0m, candle.Low);
            Assert.Equal(1523420.5m, candle.Value);
            Assert.Equal(195.0m, candle.Volume);
            Assert.Equal(new DateTime(2026, 8, 2, 0, 0, 0), candle.Begin);
            Assert.Equal(new DateTime(2026, 8, 2, 23, 59, 59), candle.End);
        }

        [Fact]
        public async Task GetCandles_BySecurityDto_ShouldUseCurrencyQueryForPreciousMetal()
        {
            // Arrange
            var jsonResponse = @"{
                ""candles"": {
                    ""columns"": [""open"", ""close"", ""high"", ""low"", ""value"", ""volume"", ""begin"", ""end""],
                    ""data"": [
                        [7000.0, 7100.0, 7150.0, 6950.0, 500000.0, 50.0, ""2026-08-02 00:00:00"", ""2026-08-02 23:59:59""]
                    ]
                }
            }";

            var handler = new MockHttpMessageHandler(jsonResponse);
            var httpClient = new HttpClient(handler);
            var factory = new StubHttpClientFactory(httpClient);

            var connector = new MoexConnector(factory);
            var security = new SecurityDto
            {
                Ticker = "GLDRUB_TOM",
                TypeId = SecurityTypeConstants.PreciousMetal
            };
            var from = new DateOnly(2026, 8, 2);
            var till = new DateOnly(2026, 8, 9);

            // Act
            var result = (await connector.GetCandlesAsync(security, from, till, 24)).ToList();

            // Assert
            Assert.Single(result);
        }

        private class StubHttpClientFactory(HttpClient client) : IHttpClientFactory
        {
            public HttpClient CreateClient(string name) => client;
        }

        private class MockHttpMessageHandler(string responseContent) : HttpMessageHandler
        {
            public Uri? LastRequestUri { get; private set; }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequestUri = request.RequestUri;
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(responseContent, Encoding.UTF8, "application/json")
                };
                return Task.FromResult(response);
            }
        }
    }
}
