using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using DataAccess.Models;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using TicketsB2C.Services.Configuration;
using TicketsB2C.Services.Purchase;
using TicketsB2C.Services.Repository;

namespace TicketsB2C.Services.Tests
{
    public class PurchaseTicketEngineTests
    {
        private readonly Mock<ITicketRepository> _ticketRepoMock = new();
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
        private readonly ExternalServicesConfiguration _config = new() { DiscountServerBaseUrl = "http://fake-discount" };

        private PurchaseTicketEngine CreateEngine(HttpResponseMessage discountResponse, Ticket? ticket = null)
        {
            // Mock ticket repository
            _ticketRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(ticket);

            // Mock HttpClient
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(discountResponse);

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

            var options = Options.Create(_config);

            return new PurchaseTicketEngine(_ticketRepoMock.Object, _httpClientFactoryMock.Object, options);
        }

        [Fact]
        public async Task PurchaseTicketAsync_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            var engine = CreateEngine(new HttpResponseMessage(HttpStatusCode.OK), ticket: null);

            var result = await engine.PurchaseTicketAsync(1, 2);

            Assert.False(result.Success);
            Assert.Equal("Ticket not found", result.Message);
            Assert.Null(result.PurchasedCost);
            Assert.Null(result.Ticket);
        }

        [Fact]
        public async Task PurchaseTicketAsync_ReturnsError_WhenDiscountServiceFails()
        {
            var ticket = new Ticket { Price = 10.0, Type = TicketType.Train };
            var engine = CreateEngine(new HttpResponseMessage(HttpStatusCode.InternalServerError), ticket);

            var result = await engine.PurchaseTicketAsync(1, 2);

            Assert.False(result.Success);
            Assert.Equal("Discount service error", result.Message);
            Assert.Null(result.PurchasedCost);
            Assert.Equal(ticket, result.Ticket);
        }

        [Fact]
        public async Task PurchaseTicketAsync_ReturnsSuccess_WhenDiscountServiceOk()
        {
            var ticket = new Ticket { Price = 10.0, Type = TicketType.Bus };
            var discountResult = new { discountedTotal = 15.0 };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(discountResult)
            };
            var engine = CreateEngine(response, ticket);

            var result = await engine.PurchaseTicketAsync(1, 2);

            Assert.True(result.Success);
            Assert.Equal("Purchase cost computed", result.Message);
            Assert.Equal(15.0, result.PurchasedCost);
            Assert.Equal(ticket, result.Ticket);
        }

        [Fact]
        public async Task PurchaseTicketAsync_CallsRepositoryWithCorrectId()
        {
            var ticket = new Ticket { Price = 10.0, Type = TicketType.Bus };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new { discountedTotal = 20.0 })
            };
            var engine = CreateEngine(response, ticket);

            await engine.PurchaseTicketAsync(42, 3);

            _ticketRepoMock.Verify(r => r.GetByIdAsync(42), Times.Once);
        }


        [Fact]
        public async Task PurchaseTicketAsync_ThrowsException_WhenDiscountServiceReturnsNullBody()
        {
            var ticket = new Ticket { Price = 10.0, Type = TicketType.Bus };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = null // No content
            };
            var engine = CreateEngine(response, ticket);

            await Assert.ThrowsAsync<JsonException>(async () => await engine.PurchaseTicketAsync(1, 2));
        }

        [Fact]
        public async Task PurchaseTicketAsync_CallsDiscountServiceWithCorrectUrl()
        {
            var ticket = new Ticket { Price = 10.0, Type = TicketType.Bus };
            var discountResult = new { discountedTotal = 15.0 };
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.RequestUri!.ToString() == "http://fake-discount/api/Discount/calculate"),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = JsonContent.Create(discountResult)
                })
                .Verifiable();

            var client = new HttpClient(handlerMock.Object);
            _httpClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);
            var options = Options.Create(_config);
            var engine = new PurchaseTicketEngine(_ticketRepoMock.Object, _httpClientFactoryMock.Object, options);

            _ticketRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ticket);

            await engine.PurchaseTicketAsync(1, 2);

            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.ToString() == "http://fake-discount/api/Discount/calculate"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task PurchaseTicketAsync_HandlesZeroQuantity()
        {
            var ticket = new Ticket { Price = 10.0, Type = TicketType.Bus };
            var discountResult = new { discountedTotal = 0.0 };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(discountResult)
            };
            var engine = CreateEngine(response, ticket);

            var result = await engine.PurchaseTicketAsync(1, 0);

            Assert.True(result.Success);
            Assert.Equal("Purchase cost computed", result.Message);
            Assert.Equal(0.0, result.PurchasedCost);
        }

    }
}
