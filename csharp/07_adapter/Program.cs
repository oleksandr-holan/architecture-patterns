using System.Text;
using System.Text.Json;

namespace _07_adapter;

public abstract class ApiClient
{
    public abstract Task<ApiResponse> GetAsync(string url, Dictionary<string, string>? headers = null);
    public abstract Task<ApiResponse> PostAsync(string url, string data, Dictionary<string, string>? headers = null);
}

public class HttpClientAdapter(HttpClient? httpClient = null) : ApiClient
{
    private readonly HttpClient _httpClient = httpClient ?? new HttpClient();

    public override async Task<ApiResponse> GetAsync(string url, Dictionary<string, string>? headers = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        AddHeaders(request, headers);
        var response = await _httpClient.SendAsync(request);
        return await AdaptResponseAsync(response);
    }

    public override async Task<ApiResponse> PostAsync(string url, string data, Dictionary<string, string>? headers = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(data, Encoding.UTF8, "application/json")
        };
        AddHeaders(request, headers);
        var response = await _httpClient.SendAsync(request);
        return await AdaptResponseAsync(response);
    }

    private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
    {
        if (headers == null) return;
        foreach (var header in headers)
        {
            request.Headers.Add(header.Key, header.Value);
        }
    }

    private static async Task<ApiResponse> AdaptResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var contentType = response.Content.Headers.ContentType?.MediaType;
        return new ApiResponse
        {
            StatusCode = (int)response.StatusCode,
            Headers = response.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)),
            Body = contentType == "application/json" ? JsonSerializer.Deserialize<JsonElement>(content) : content
        };
    }
}

public class ApiResponse
{
    public int StatusCode { get; init; }
    public required Dictionary<string, string> Headers { get; set; }
    public required object Body { get; init; }
}

public static class UserService
{
    public static async Task<JsonElement> FetchUserDataAsync(ApiClient apiClient, int userId)
    {
        var response = await apiClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{userId}");
        if (response.StatusCode == 200)
        {
            return (JsonElement)response.Body;
        }
        else
        {
            throw new Exception($"Failed to fetch user data: {response.StatusCode}");
        }
    }

    public static async Task<JsonElement> CreateUserAsync(ApiClient apiClient, string userData)
    {
        var response = await apiClient.PostAsync("https://graph.microsoft.com/v1.0/users", userData);
        if (response.StatusCode == 201)
        {
            return (JsonElement)response.Body;
        }
        else
        {
            throw new Exception($"Failed to create user: {response.StatusCode}");
        }
    }
}

internal static class Program
{
    private static async Task Main()
    {
        var apiClient = new HttpClientAdapter();

        try
        {
            var user = await UserService.FetchUserDataAsync(apiClient, 1);
            Console.WriteLine($"Fetched user: {user}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        var data = new
        {
            accountEnabled = true,
            displayName = "Adele Vance",
            mailNickname = "AdeleV",
            userPrincipalName = "AdeleV@contoso.com",
            passwordProfile = new
            {
                forceChangePasswordNextSignIn = true,
                password = "xWwvJ]6NMw+bWH-d"
            }
        };
        var dataJson = JsonSerializer.Serialize(data);

        try
        {
            var newUser = await UserService.CreateUserAsync(apiClient, dataJson);
            Console.WriteLine($"Created user: {newUser}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}