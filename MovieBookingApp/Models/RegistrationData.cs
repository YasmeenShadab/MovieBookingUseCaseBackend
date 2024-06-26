using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace MovieBookingApp.Models
{
    public class RegistrationData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public ObjectId RegistrationId { get; set; }

        [Required(ErrorMessage ="Please Enter your FirstName")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage ="Please Enter your Last Name")]
        public string? LastName { get; set; }

        [Required(ErrorMessage ="Please Enter EmailID")]
        [EmailAddress(ErrorMessage ="Invalid EmailID")]
        public string? Email { get; set; }

        [Required(ErrorMessage ="Please Enter LoginID")]
        public string? LoginId { get; set; }

        [Required(ErrorMessage ="Please Enter Password")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="Please Enter Confirm Password")]
        public string? ConfirmPassword { get; set;}

        [Required(ErrorMessage ="Please Enter Phone Number")]
        [RegularExpression(@"^[6789]\d{9}$", ErrorMessage ="Invalid Phone Number")]
        public string? PhoneNumber { get; set; }
    }
}
