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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("abcd")]
    [InlineData("qqgyP7wKDPu1Z0dFWMiaBRbRY8vkvX6BD2z23MQfPlheyqEpak9Xwh2PNx6VSNUnILgG1uWCZM0dlSJRYgNnGD4pFcREu3ttc7K6tusL0zmrAStXfxR6VpDgjOc9idKAa1YuPcRLpD1R7YbfbHM5osP797wB5JDjUyWwYghOQaCb89R9XIszrTPZgOyo6aWBeKdzgmOYoLmRN3Zh3DaLRoshM2N9RPVyWGrAuYchRFBIWF9rcwFP6uvInC66JE9JLw1lfZKY4ewlDBZaNcuC4NQx1BkBOl6RyfDNqN8XrDQVRMRl8s4SinPju49eIoNdyJ31u9M0PHGnHHOUzkrpwSJHcUcwD5xE1jSrhgidbl4Mz2KMDy3Ioh4COz2TN7YZp920rNWAqqoyp5DXPwGTWpCbNimpLqYSfWFRuu8Rq7dg3W5opi1ZSWWEIkrr03cXbIbSYj49Bb5hxpwxm8Oi9qrindsacEoXUaETOq1pLtLCz0JTU4woanm9QJ0f95e52KqN2GHj6U6lG9MmKpo9m4pblyjgq4jfJxljWB0eraHuEH1fey0uKJqAf1D1YEXmRVWTwlfXMfQBAZo9ZE7nAbkBDZ15RYAA7lePtnQNtGs459HXo5PGj5VGzql9pRtn0B7ZjCclR3gr9Sz7emRogakuwThICBiSpl2Z18YE3xCWUaSfqkDbDqNvkAfHcsWkba5r7YU2GuCE0P4dHbcuYcFXnjUrvF6bOKnKRGJ2O4H497dijMYAOg1UwEJlwsUNTqnzvvtJA8I9hxYBzGXqod4OEwPzMK1IRQ0aDYOl2DtcHaTzxX2uzdtQMqAkPwmH80LmobqjCFKf71dF76I9Rp0jJtW72KebIXG7fH8ein5NUzEe1B43mUL2Ml33zmKAFTY04j7vRZ5R2bS830P3l9TsbnCgMZcwlMQuEVQu489Z0ICtCMDLnIOSTvYIlNQQt8vG2mQW6emtDmuRPRKa6fHcrwMriVpFK0d00fzGhWyzn6f3B2pKN0Mtai3aGwvvmGMloVd6KBS9whXJEtiAbbAta8RFyjaOQL52Gx4Zn2QIJ6iycgKzDUeI7KYTtzKYOoxQz2fE7LQ93P3wcIeYwPnR9j7HJvlXa20UPbT8oXRm8mqR9xJIgWSG3EfvaC5wc1As0nLlidXFTM5NOZcBxJ0QaKr3ycBkmfpTkSnyTY6C2p2hD5HLDYejfNyypeicchn275RbSaz8VljWMSNeq1EOld6P5Gf5hqcs99LBEbxrHThlU0MaeSAPPnSQKX1UHxXkPU3fN1vUaPbxjj0gRwwD1WhVV9TQ0ZHjWDMFSZ5uo7NklnSn8wZbrQu8jZN3qmBkBdVgf6Xibk4gzLS1LpyrvAKQMMMgHrfuVmm1GxKdZuzXUKoMZ6SET9PqrplDBQBq1A1aQv51cvDhb25JTJvOHBGKNkZf3wDah2HUgLTELeagGVbu4lkVHvQ4qeqBYwfs2DVBwPIHIRDuCBlYaWrg4Rsbz94qanNTMZa4QSDa5kHs4o77Jp81UvvkBU4i5FgnhKzOhQLPyYvbc26r7wZFwNXp1WBWjKJxzehUwRwZNE5MzQqrQqYPvUjINsGrUJpfxs7wLmee4vJRor3byeebiNTRcIunuQKHYomQa0Zj5zpW0bgpxtrC9oAYQgipLxsxJ1mtTeNpAPTwzydUkkl7JfH15YmvaHYJvQSSdJoD625zisXW9dQNM0aMc0qGxc5ONtRzo0bB5168j1zSpkG8QqJ5sjl2I33bWbtbaX9t04pw3cm116w74id7FH4tpJ1WL7yl9FX3dyX757kPB8sk8eBjd3TTfHqOjUnl5p96r1wq23QRHN4aQMIQiWxjStQMrzIfifH9lTtp5RtA9cX1i02BivkBtP7qMXVMcdYOCIJA1adOZQqqxyeTLrHUvrNPwwkvUp3TCT9RtpHgk9sSsmKDvDFehKbmzcBD3qVGspE3Qs1vRcPHoeVgZySmaTZoAgxhj5z6rlANClcGWYgRWoU4zALRsYio9n6WW6wK29mwwGfsL0hDs6Mc2eL6jN202RBqBwbc3bmoykjMlk01cJWttFkPwyFE43GnbVtWXZaGEA1h")]
    public async Task CreateUrlShouldThrowException(string originUrl)
    {
        //Arrange
        var request = new CreateUrlRequest
        {
            OriginUrl = originUrl
        };

        //Act
        using var response = await _client.PutAsJsonAsync("api/url", request);

        //Assert
        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<Error>();
        result.Code.ShouldBe(ErrorCode.BadRequestError);
    }

}