namespace LibraryOfAlexandria.Models
{
    public class Game
    {
        private string Title { get; set; }
        private bool IsMultiplayer { get; set; }
        // Make enum for all game tags
        //private List<Tags> GameTags { get; set; }
        private string StorePageLink { get; set; }
        private int TimePlayed { get; set; }
    }
}
