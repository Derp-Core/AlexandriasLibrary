using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryOfAlexandria.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        private string UserId { get; set; } = "";
        // Uncomment when game class is created
        //private List<Game> OwnedGames { get; set; }
        //private List<Game> WishListGames { get; set; }
        private List<User>? FriendsList { get; set; }
    }
}
