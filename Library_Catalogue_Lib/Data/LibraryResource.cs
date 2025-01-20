using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Library_Catalogue_Lib.Data
{
    public class LibraryResource
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("genres")]
        public List<string> Genres { get; set; }

        [BsonElement("format")]
        public string Format { get; set; }

        [BsonElement("available")]
        public bool Available { get; set; }

        [BsonElement("borrow_limit")]
        public int BorrowLimit { get; set; }

        [BsonElement("useOnlyInLibrary")]
        public bool UseOnlyInLibrary { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("available_quantity")]
        public int AvailableQuantity { get; set; }

        [BsonElement("not_available_quantity")]
        public int NotAvailableQuantity { get; set; }

        [BsonElement("cataloged_by")]
        public CatalogedBy CatalogedBy { get; set; }

        [BsonElement("publisher")]
        public string Publisher { get; set; } 

        [BsonElement("author")]
        public List<string> Author { get; set; } 

        [BsonElement("language")]
        public string Language { get; set; } 

        [BsonElement("publication_date")]
        public DateTime PublicationDate { get; set; } 

        [BsonElement("edition")]
        public string Edition { get; set; } 

        [BsonElement("tags")]
        public List<string> Tags { get; set; } 

        [BsonElement("description")]
        public string Description { get; set; } 

        [BsonElement("location")]
        public string Location { get; set; }
    }

    public class CatalogedBy
    {
        [BsonElement("userID")]
        public string UserID { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
