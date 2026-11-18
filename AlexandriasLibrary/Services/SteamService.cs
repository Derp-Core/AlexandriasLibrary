namespace AlexandriasLibrary.Services
{
    using AlexandriasLibrary.Models;
    using System.Text.Json;

    public class SteamService
    {
        private readonly HttpClient _http;
        private readonly string _key;

        public SteamService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _key = config["Steam:ApiKey"] ?? throw new Exception("Missing Steam:ApiKey");
        }

        public async Task<(int Earned, int Total)?> GetAchievementsAsync(string steamId, int appId)
        {
            var resp = await _http.GetAsync(
                $"https://api.steampowered.com/ISteamUserStats/GetPlayerAchievements/v1/?key={_key}&steamid={steamId}&appid={appId}");
            if (!resp.IsSuccessStatusCode) return null; // no achievements or not visible

            var json = await resp.Content.ReadAsStringAsync();
            var root = JsonDocument.Parse(json).RootElement;
            if (!root.TryGetProperty("playerstats", out var ps) ||
                !ps.TryGetProperty("achievements", out var list)) return null;

            int total = 0, earned = 0;
            foreach (var a in list.EnumerateArray())
            {
                total++;
                if (a.GetProperty("achieved").GetInt32() == 1) earned++;
            }
            return total == 0 ? null : (earned, total);
        }

        public async Task<string?> ResolveSteamIdAsync(string input)
        {
            input = input.Trim().TrimEnd('/');

            // If it's a link, pull out the part we need
            if (input.Contains("steamcommunity.com", StringComparison.OrdinalIgnoreCase))
            {
                if (!input.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    input = "https://" + input;

                if (Uri.TryCreate(input, UriKind.Absolute, out var uri))
                {
                    var parts = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2 &&
                        (parts[0].Equals("id", StringComparison.OrdinalIgnoreCase) ||
                         parts[0].Equals("profiles", StringComparison.OrdinalIgnoreCase)))
                    {
                        input = parts[1];
                    }
                }
            }

            // Already a SteamID64
            if (input.Length == 17 && input.All(char.IsDigit)) return input;

            // Otherwise treat it as a custom URL name
            var json = await _http.GetStringAsync(
                $"https://api.steampowered.com/ISteamUser/ResolveVanityURL/v1/?key={_key}&vanityurl={Uri.EscapeDataString(input)}");
            var r = JsonDocument.Parse(json).RootElement.GetProperty("response");
            return r.GetProperty("success").GetInt32() == 1 ? r.GetProperty("steamid").GetString() : null;
        }

        public async Task<List<OwnedGame>> GetOwnedGamesAsync(string steamId)
        {
            var json = await _http.GetStringAsync(
                $"https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={_key}&steamid={steamId}&include_appinfo=true&include_played_free_games=true");
            var r = JsonDocument.Parse(json).RootElement.GetProperty("response");
            if (!r.TryGetProperty("games", out var games)) return new(); // private profile

            return games.EnumerateArray().Select(g => new OwnedGame
            {
                AppId = g.GetProperty("appid").GetInt32(),
                Name = g.GetProperty("name").GetString() ?? "",
                PlaytimeMinutes = g.GetProperty("playtime_forever").GetInt32()
            }).ToList();
        }
    }
}