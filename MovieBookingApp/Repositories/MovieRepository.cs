using MongoDB.Bson;
using MongoDB.Driver;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Repositories
{
    public class MovieRepository:IMovieRepository
    {
        private readonly IMongoCollection<Movie> _movies;
        private readonly IMongoCollection<Tickets> _tickets;
        private readonly IConfiguration _configuration;
        public MovieRepository(IMongoClient mongoClient,IConfiguration configuration)
        {
            _configuration = configuration;
            string? dbName = configuration["Database:Name"];
            var database = mongoClient.GetDatabase(dbName);
            _movies = database.GetCollection<Movie>(nameof(Movie)); ;
            _tickets = database.GetCollection<Tickets>(nameof(Tickets));
        }

        #region Add Movie
        /// <summary>
        /// Add Movie Details
        /// </summary>
        /// <param name="movie"></param>
        /// <returns>
        /// Movie Id
        /// </returns>
        public async Task<ObjectId> AddMovie(Movie movie)
        {
            try
            {
                await _movies.InsertOneAsync(movie);
               
                return movie.MovieId;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine("Exception Occured while adding Movie",ex.Message);
                
                return ObjectId.Empty;
            }
        }
        #endregion

        #region Get All movies
        /// <summary>
        /// Get ALl Movies
        /// </summary>
        /// <returns>
        /// Get All Movie Details
        /// </returns>
        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            try
            {
                var movies = await _movies.Find(_ => true).ToListAsync();
                
                return movies;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception Ouccured while searching all Movies",ex.Message);
                
                return Enumerable.Empty<Movie>();
            }
        }
        #endregion

        #region Get Movie By Name
        /// <summary>
        /// Get Movie By Name
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns>
        /// Movie Details
        /// </returns>
        public async Task<IEnumerable<Movie>> GetMovieByName(string movieName)
        {
            try
            {
                var filter = Builders<Movie>.Filter.Eq(x => x.MovieName, movieName);
                var movie = await _movies.Find(filter).ToListAsync();
                
                return movie;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception Occured while getting the movie", ex.Message);

                return Enumerable.Empty<Movie>();
            }
        }
        #endregion

        #region Update Movie
        /// <summary>
        /// Updating Movie status
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns>
        /// true or false
        /// </returns>
        public async Task<bool> UpdateMovie(string movieName)
        {
            try
            {
                var filter = Builders<Movie>.Filter.And(
                    Builders<Movie>.Filter.Eq(x => x.MovieName, movieName),
                    Builders<Movie>.Filter.Eq(x => x.TotalTickets, 0)
                    );
                var update = Builders<Movie>.Update.Set(x => x.Status, "Booking Closed");
                var result = await _movies.UpdateOneAsync(filter, update);

                return result.ModifiedCount == 1;
            }
            catch(Exception ex) 
            {
                Console.WriteLine("Exception Occured while updating the Movie", ex.Message);

                return false;
            }
        }
        #endregion

        #region Delete Movie
        /// <summary>
        /// Delete Movie
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns>
        /// true or false
        /// </returns>
        public async Task<bool> DeleteMovie(string movieName)
        {
            try
            {
                var filter = Builders<Movie>.Filter.Eq(x => x.MovieName, movieName);
                var result = await _movies.DeleteOneAsync(filter);

                return result.DeletedCount == 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception Occured while deleting the Movie", ex.Message);

                return false;
            }
        }
        #endregion

        #region Book Movie
        /// <summary>
        /// Book Movie Ticket
        /// </summary>
        /// <param name="book"></param>
        /// <returns>
        /// Ticket Id
        /// </returns>
        public async Task<ObjectId> BookMovie(Tickets book)
        {
            try
            {
                var filter = Builders<Movie>.Filter.Where(m => m.MovieName == book.MovieName && m.Theatre == book.Theater);
                var movie = await _movies.Find(filter).FirstOrDefaultAsync();
                
                if (movie == null || movie.TotalTickets < book.NumberOfTicketsBooked)
                {
                    return ObjectId.Empty;
                }
                
                var update = Builders<Movie>.Update.Inc(m => m.TotalTickets, -book.NumberOfTicketsBooked);
                var movieUpdateResult = await _movies.UpdateOneAsync(filter, update);
                await _tickets.InsertOneAsync(book);

                return book.Id;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception Occured while Booking Ticket", ex.Message);
                
                return ObjectId.Empty;
            }
        }
        #endregion

    }
}
