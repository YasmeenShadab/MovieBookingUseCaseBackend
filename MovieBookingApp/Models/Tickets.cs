using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MovieBookingApp.Models
{
    public class Tickets
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId MovieId { get; set; }
        public string UserName { get; set; }
        public string MovieName { get; set; }
        public string Theater { get; set;}
        public int NumberOfTicketsBooked { get; set;}
    }
}
