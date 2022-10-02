using Microsoft.AspNetCore.Mvc.Testing;

namespace UrlShortener.IntegrationTests;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Shouldly;

using UrlShorteneer.Contracts.Http;

using Xunit;



public class UrlApiTests
{

    private readonly HttpClient _client;
    public UrlApiTests()
    {
        var application = new WebApplicationFactory<Program>();

        _client = application.CreateClient();
    }

    [Fact]
    public async Task CreateUrlShouldReturnUrlWithGeneretedIdAndShortUrl()
    {
        //Arrange
        var request = new CreateUrlRequest
        {
            OriginUrl = "https://www.myinstants.com/"
        };


        //Act
        using var response = await _client.PutAsJsonAsync("api/url", request);
        //Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Created);

        var result = await response.Content.ReadFromJsonAsync<CreateUrlResponse>();

        result.Id.ShouldBeGreaterThan(0);
        result.ShortenedUrl.ShouldNotBeNullOrEmpty();

    }
}