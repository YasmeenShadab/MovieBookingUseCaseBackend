using MongoDB.Bson;
using MovieBookingApp.Models;

namespace MovieBookingApp.Interfaces
{
    public interface IMovieRepository
    {
        Task<ObjectId> AddMovie(Movie movie);
        Task<IEnumerable<Movie>> GetAllMovies();
        Task<IEnumerable<Movie>> GetMovieByName(string movieName);
        Task<bool> UpdateMovie(string movieName);
        Task<bool> DeleteMovie(string movieName);
        Task<ObjectId> BookMovie(Tickets book);
    }
}
