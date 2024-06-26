using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;

namespace MovieBookingApp.Controllers
{
    [Route("api/v1.0/moviebooking")]
    [ApiController]
    public class MovieBookingController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MovieBookingController> _logger;
        public MovieBookingController(IMovieRepository movieRepository, ILogger<MovieBookingController> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get All Movies
        /// </summary>
        /// <returns>
        /// All Movies 
        /// </returns>
        [HttpGet("GetAllMovies")]
        public async Task<JsonResult> GetAllMovies()
        {
            _logger.LogInformation("Get all Movies : Moovie Booking Controller");
            var movie = await _movieRepository.GetAllMovies();
            
            return new JsonResult(movie);
        }

        /// <summary>
        /// Get Movie By Name
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns>
        /// Movie details
        /// </returns>
        [HttpGet("searchbyname")]
        public async Task<JsonResult> GetMovieByName(string movieName)
        {
            if (movieName != null)
            {
                _logger.LogInformation("Get Movie By Name : Movie Booking Controller");
                _logger.LogDebug($"Movie name: {movieName}");
                var movie = await _movieRepository.GetMovieByName(movieName);

                return new JsonResult(movie);
            }
            _logger.LogInformation("Get Movie By Name : Movie Name is Null");
            return new JsonResult("Unable to get movie");
        }

        /// <summary>
        /// Add Movie
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// Movie Id
        /// </returns>
        [HttpPost("add")]
        public async Task<JsonResult> AddMovie([FromBody] Movie data)
        {
            if (data != null)
            {
                _logger.LogInformation("Add Movie  : Movie Booking Controller");
                var id = await _movieRepository.AddMovie(data);
                if (id.ToString() == "")
                {
                    return new JsonResult("Add not success");
                }
                return new JsonResult("Added Succesfully");
            }
            _logger.LogInformation("Add Movie  : Movie data is null");

            return new JsonResult("Unable to Add Movie");
        }

        /// <summary>
        /// Update Movie 
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns>
        /// true or false
        /// </returns>
        [HttpPut("update")]
        public async Task<JsonResult> UpdateMovie(string movieName)
        {
            if (movieName != null)
            {
                _logger.LogInformation("Update Movie : Movie Booking Controller");
                var movies = await _movieRepository.UpdateMovie(movieName);

                if (!movies)
                {
                    return new JsonResult("Movie already Updated");
                }
                return new JsonResult("Movie Updated");
            }
            _logger.LogInformation("Update Movie : Movie Name is Null");
            return new JsonResult("Unable to Update Movie");
        }

        /// <summary>
        /// Delete Movie
        /// </summary>
        /// <param name="movieName"></param>
        /// <returns>
        /// true or false
        /// </returns>
        [HttpDelete("delete")]
        public async Task<JsonResult> DeleteMovie(string movieName)
        {
            if (movieName != null)
            {
                _logger.LogInformation("Delete Movie : Movie Booking Controller");
                var movie = await _movieRepository.DeleteMovie(movieName);

                if (!movie)
                {
                    return new JsonResult("Movie already Deleted");
                }
                return new JsonResult("Movie Deleted");
            }
            _logger.LogInformation("Delete Movie : Movie Name is null");

            return new JsonResult("Unable to Delete Movie");
        }

        /// <summary>
        /// Book the ticket
        /// </summary>
        /// <param name="data"></param>
        /// <returns>
        /// Ticket Id
        /// </returns>
        [HttpPost("Book")]
        public async Task<JsonResult> BookMovie([FromBody] Tickets data)
        {
            if (data != null)
            {
                _logger.LogInformation("Book Movie : Movie Booking Controller");
                var id = await _movieRepository.BookMovie(data);

                if (id == ObjectId.Empty)
                {
                    return new JsonResult("Can not book tickets, please check number Of tickets available");
                }

                return new JsonResult(id.ToString() != null ? "Tickect Booked Sucessfully" : "Please Try Again");
            }
            _logger.LogInformation("Book Movie :Ticket data is null");
            return new JsonResult("Unable to book Movie");
        }

    }
}
