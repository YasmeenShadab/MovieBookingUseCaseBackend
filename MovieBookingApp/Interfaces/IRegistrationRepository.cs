using MongoDB.Bson;
using MovieBookingApp.Models;

namespace MovieBookingApp.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<ObjectId> RegistreUser(RegistrationData data);
        Task<bool> LoginUser(string loginId,string password);
        Task<bool> ForgetPassword(SetPassword setPassword);
        Task<string> GetEmailId(RegistrationData data);
        Task<string> GetLoginId(RegistrationData data);
    }
}
