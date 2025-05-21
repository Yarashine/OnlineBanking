using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class ClientMongoConfiguration
{
    public static void Configure()
    {
        var pack = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreIfNullConvention(true),
        };
        ConventionRegistry.Register("UserServiceConventions", pack, _ => true);
        BsonClassMap.RegisterClassMap<Client>(cm =>
        {
            cm.AutoMap();
            cm.MapIdField(c => c.Id)
              .SetIdGenerator(StringObjectIdGenerator.Instance)
              .SetSerializer(new StringSerializer(BsonType.ObjectId));
        });
    }
}
