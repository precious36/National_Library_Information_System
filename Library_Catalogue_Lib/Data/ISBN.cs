using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Catalogue_Lib.Data
{
    public class ISBN
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("isbn")]
        public string IsbnCode { get; set; }

        [BsonElement("resource_id")]
        public ObjectId ResourceId { get; set; }

        [BsonElement("library_id")]
        public ObjectId LibraryId { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
