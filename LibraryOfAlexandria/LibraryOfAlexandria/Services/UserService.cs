using MongoDB.Driver;
using LibraryOfAlexandria.Models;

namespace LibraryOfAlexandria.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> library;

        public UserService(IMongoClient mongoClient, IConfiguration config)
        {
            string databaseName = config["MongoDb:ConnectionString"];
            var database = mongoClient.GetDatabase(databaseName);
            library = database.GetCollection<User>("Library");
        }

        public async Task<List<User>> GetUsersAsync() =>
            await library.Find(_ => true).ToListAsync();

        public async Task CreateUsersAsync(User newUser) =>
            await library.InsertOneAsync(newUser);
    }
}
