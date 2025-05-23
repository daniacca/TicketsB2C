using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Discount.Tests
{
    public class DiscountApiE2eTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public DiscountApiE2eTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CalculateDiscount_ReturnsExpectedResult()
        {
            var request = new
            {
                Quantity = 20,
                Price = 10.0,
                Total = 200.0,
                Type = "Bus"
            };

            var response = await _client.PostAsJsonAsync("/api/Discount/calculate", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DiscountResultDto>();
            result.Should().NotBeNull();
            result!.discountedTotal.Should().BeLessThan(200.0);
        }

        [Fact]
        public async Task CalculateDiscount_ReturnsBadRequest_WhenInvalidInput()
        {
            var request = new
            {
                Quantity = -1,
                Price = 10.0,
                Total = 200.0,
                Type = "Bus"
            };
            var response = await _client.PostAsJsonAsync("/api/Discount/calculate", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CalculateDiscount_ReturnsSameAmount_WhenNoDiscountApplied()
        {
            var request = new
            {
                Quantity = 2,
                Price = 10.0,
                Total = 20.0,
                Type = "UnknownType"
            };

            var response = await _client.PostAsJsonAsync("/api/Discount/calculate", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<DiscountResultDto>();
            result.Should().NotBeNull();
            result!.discountedTotal.Should().Be(20.0);
        }

        public class DiscountResultDto
        {
            public double discountedTotal { get; set; }
        }
    }
}