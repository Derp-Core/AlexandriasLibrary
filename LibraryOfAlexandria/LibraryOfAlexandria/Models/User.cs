using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LibraryOfAlexandria.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        private string UserId { get; set; }
        // Uncomment when game class is created
        //private Game[] OwnedGames { get; set; }
        //private Game[] WishListGames { get; set; }
        private User[] FriendsList { get; set; }
    }
}
