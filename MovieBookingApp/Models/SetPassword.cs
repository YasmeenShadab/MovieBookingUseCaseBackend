namespace MovieBookingApp.Models
{
    public class SetPassword
    {
        public string LoginID { get; set; }
        public string Email { get; set; }    
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set;}
    }
}
