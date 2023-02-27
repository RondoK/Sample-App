using System.Net;
using FluentAssertions;

namespace Api.Tests;

public static class Tools
{
    public static async Task GetAndCheckRedirected(this HttpClient client, string path)
    {
        var result = await client.GetAsync(path);
        result.StatusCode.Should().Be(HttpStatusCode.Redirect);
    }

    public static async Task ShouldBeOk(this HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.InternalServerError)
            throw new Exception(await response.Content.ReadAsStringAsync());

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> CookieHeaders(
        this HttpResponseMessage message)
        => message.Headers.Where(header => header.Key == "Set-Cookie");
}