using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_Catalogue_Lib.Data
{
    public class Publisher
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; } 

        [BsonElement("contact_number")]
        public string ContactNumber { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } 

        [BsonElement("website")]
        public string Website { get; set; }

        [BsonElement("established_date")]
        public DateTime EstablishedDate { get; set; } 

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

}
