using MongoDB.Bson;
using MongoDB.Driver;
using Datyche.Models;

namespace Datyche.Controllers
{
    public class MongoUtils
    {
        internal static IMongoCollection<User> GetUsersCollection()
        {
            var client = new MongoClient(Environment.GetEnvironmentVariable("DATYCHE_DB_DSN"));
            var database = client.GetDatabase("datyche");
            var collection = database.GetCollection<User>("users");
            return collection;
        }
    }
}