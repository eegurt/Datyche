using MongoDB.Bson;
using MongoDB.Driver;

namespace Datyche.Controllers
{
    public class MongoUtils
    {
        internal static IMongoCollection<BsonDocument> GetDBUsersCollection()
        {
            var client = new MongoClient("mongodb+srv://egurt:truge@datyche.yhsit18.mongodb.net/test");
            var database = client.GetDatabase("datyche");
            var collection = database.GetCollection<BsonDocument>("users");
            return collection;
        }
    }
}