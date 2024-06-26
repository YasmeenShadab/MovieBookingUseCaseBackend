using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class Movie
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public ObjectId MovieId { get; set; }

        [Required(ErrorMessage ="Please enter MovieName")]
        public string MovieName { get; set; }
        public int TotalTickets { get; set; }
        public string Theatre { get; set; }
        public string Status { get; set; }
    }
}
