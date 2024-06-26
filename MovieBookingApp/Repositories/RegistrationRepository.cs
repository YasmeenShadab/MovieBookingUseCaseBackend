using MongoDB.Bson;
using MongoDB.Driver;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly IMongoCollection<RegistrationData> _registries;
        private readonly IConfiguration _configuration;
        public RegistrationRepository(IMongoClient mongoClient,IConfiguration configuration)
        {
            _configuration = configuration;
            string? dbName = configuration["Database:Name"];
            var database = mongoClient.GetDatabase(dbName);
            var collection = database.GetCollection<RegistrationData>(nameof(RegistrationData));

            _registries = collection;
        }

        #region Forget Password Method
        /// <summary>
        /// Reset your forgotten Password
        /// </summary>
        /// <param name="setPassword"></param>
        /// <returns>
        /// tru or false
        /// </returns>
        public async Task<bool> ForgetPassword(SetPassword setPassword)
        {
            try
            {
                var filter = Builders<RegistrationData>.Filter.And(
                    Builders<RegistrationData>.Filter.Eq(x => x.LoginId, setPassword.LoginID),
                    Builders<RegistrationData>.Filter.Eq(x => x.Email, setPassword.Email)
                    );
                var update = Builders<RegistrationData>.Update.Combine(
                    Builders<RegistrationData>.Update.Set(x => x.Password, setPassword.NewPassword),
                    Builders<RegistrationData>.Update.Set(x => x.ConfirmPassword, setPassword.ConfirmNewPassword)
                    );
                var result = await _registries.UpdateOneAsync(filter, update);
                
                return result.ModifiedCount == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured while retting the password",ex.Message);
                
                return false;
            }
        }
        #endregion

        #region Login User Method
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="password"></param>
        /// <returns>
        /// true or false
        /// </returns>
        public async Task<bool> LoginUser(string loginId,string password)
        {
            try
            {
                var filter = Builders<RegistrationData>.Filter.And(
                    Builders<RegistrationData>.Filter.Eq(x => x.LoginId, loginId),
                    Builders<RegistrationData>.Filter.Eq(x => x.Password, password)
                    );
                var user = await _registries.Find(filter).ToListAsync();
                if (user.Count == 0)
                {
                    return false;
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occured while login : ", ex.Message);
                return false;
            }
        }
        #endregion

        #region Register User Method
        /// <summary>
        /// Register User
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// Registration Id
        /// </returns>
        public async Task<ObjectId> RegistreUser(RegistrationData data)
        {
            try
            {
                await _registries.InsertOneAsync(data);
                
                return data.RegistrationId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occured while registering: ", ex.Message);
                return ObjectId.Empty;
            }
        }
        #endregion

        #region Get Email Id Method
        /// <summary>
        /// Get Email Id
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// string
        /// </returns>
        public async Task<string> GetEmailId(RegistrationData data)
        {
            try
            {
                var filter = Builders<RegistrationData>.Filter.And(
                    Builders<RegistrationData>.Filter.Eq(x => x.Email, data.Email)
                    );
                var user = await _registries.Find(filter).ToListAsync();
                if (user.Count == 0)
                {
                    return "success";
                }
                return "fail";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception while getting emailId", ex.Message);
                return "fail";
            }
        }
        #endregion

        #region Get Login ID Method
        /// <summary>
        /// Get Login Id
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// String
        /// </returns>
        public async Task<string> GetLoginId(RegistrationData data)
        {
            try
            {
                var filter = Builders<RegistrationData>.Filter.And(
                    Builders<RegistrationData>.Filter.Eq(x => x.LoginId, data.LoginId)
                    );
                var user = await _registries.Find(filter).ToListAsync();
                if (user.Count == 0)
                {
                    return "success";
                }
                return "fail";
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occured while getting Login Id", ex.Message);
                return "fail";
            }
        }
        #endregion
    }
}
