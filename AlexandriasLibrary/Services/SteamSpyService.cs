using System.Text.Json;

namespace AlexandriasLibrary.Services;

public class SteamSpyService
{
    private readonly HttpClient _http;
    public SteamSpyService(HttpClient http) => _http = http;

    public async Task<List<string>> GetTopTagsAsync(int appId, int count = 5)
    {
        var json = await _http.GetStringAsync(
            $"https://steamspy.com/api.php?request=appdetails&appid={appId}");
        var root = JsonDocument.Parse(json).RootElement;

        // SteamSpy returns [] instead of {} when a game has no tags
        if (!root.TryGetProperty("tags", out var tags) ||
            tags.ValueKind != JsonValueKind.Object) return new();

        return tags.EnumerateObject()
            .OrderByDescending(t => t.Value.GetInt32())
            .Take(count)
            .Select(t => t.Name)
            .ToList();
    }
}