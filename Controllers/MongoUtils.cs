using MongoDB.Bson;
using MongoDB.Driver;
using Datyche.Models;

namespace Datyche.Controllers
{
    public class MongoUtils
    {
        internal static IMongoCollection<User> GetDBUsersCollection()
        {
            var client = new MongoClient("mongodb+srv://egurt:truge@datyche.yhsit18.mongodb.net/test");
            var database = client.GetDatabase("datyche");
            var collection = database.GetCollection<User>("users");
            return collection;
        }
    }
}