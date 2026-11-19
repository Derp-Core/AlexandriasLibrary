namespace AlexandriasLibrary.Models
{
    public class OwnedGame
    {
        public int AppId { get; set; }
        public string Name { get; set; } = "";
        public int PlaytimeMinutes { get; set; }
        public string HeaderUrl => $"https://cdn.cloudflare.steamstatic.com/steam/apps/{AppId}/header.jpg";
        public int? AchievementsEarned { get; set; }
        public int? AchievementsTotal { get; set; }
        public List<string> Tags { get; set; } = new();
    }
}
