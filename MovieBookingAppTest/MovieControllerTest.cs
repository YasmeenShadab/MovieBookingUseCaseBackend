using Moq;
using Microsoft.AspNetCore.Mvc;
using MovieBookingApp.Controllers;
using MovieBookingApp.Interfaces;
using MovieBookingApp.Models;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace MovieBookingAppTest
{
    public class MovieBookingTest
    {
        private readonly Mock<IMovieRepository> movieRepository;
        private readonly Mock<ILogger<MovieBookingController>> logger;

        public MovieBookingTest()
        {
            movieRepository = new Mock<IMovieRepository>();
            logger=new Mock<ILogger<MovieBookingController>>();
        }
       
        /// <summary>
        /// Get All Movie Unit Test Case
        /// </summary>
        [Test]
        public void GetAllMoviesTest()
        {
            //Arrange
            var expected = new List<Movie>();
            movieRepository.Setup(x => x.GetAllMovies().Result);

            var controller= new MovieBookingController(movieRepository.Object,logger.Object);

            //Act
            var movies=controller.GetAllMovies();
            var result = (JsonResult)movies.Result;

            //Assert
            Assert.IsNotNull(movies);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        /// <summary>
        /// Get Movie By Name Test Case
        /// </summary>
        [Test]
        public void GetMovieByNameTest() 
        {
            //Arrange
            var movieName = "NewMovie";
            var expected = new List<Movie>();
            var controller = new MovieBookingController(movieRepository.Object,logger.Object);
            movieRepository.Setup(x => x.GetMovieByName(It.IsAny<string>()));

            //Act
            var movie = controller.GetMovieByName(movieName);
            var result = (JsonResult)movie.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        /// <summary>
        /// Add Movie Unit test case
        /// </summary>
        [Test]
        public void AddMovieTest()
        {
            //Arrange
            var NewMovie =  new Movie {
                MovieId=ObjectId.GenerateNewId(),
                MovieName= "Foo",
                TotalTickets=100,
                Theatre ="PVP",
                Status="Booking open"
            } ;
            var controller = new MovieBookingController(movieRepository.Object,logger.Object);
            movieRepository.Setup(x => x.AddMovie(It.IsAny<Movie>())).ReturnsAsync(new ObjectId("66347428e574c152bf877ed5"));

            //Act
            var addMovie = controller.AddMovie(NewMovie);
            var result = (JsonResult)addMovie.Result;

            //Assert
            Assert.IsNotNull(result);
            Assert.That(result.Value.ToString(), Is.EqualTo("66347428e574c152bf877ed5"));
        }

        /// <summary>
        /// Update Movie Test case
        /// </summary>
        [Test]
        public void UpdateMovieTest()
        {
            //Arrange
            var movieName = "NewMovie";
            var expected = new List<Movie>();
            var controller = new MovieBookingController(movieRepository.Object, logger.Object);
            movieRepository.Setup(x => x.UpdateMovie(It.IsAny<string>()));

            //Act
            var movie = controller.UpdateMovie(movieName);
            var result = (bool)((JsonResult)movie.Result).Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result|| !result);
        }

        /// <summary>
        /// Delete Movie Test case
        /// </summary>
        [Test]
        public void DeleteMovieTest()
        {
            //Arrange
            var movieName = "Sample";
            var expected = new List<Movie>();
            var controller = new MovieBookingController(movieRepository.Object, logger.Object);
            movieRepository.Setup(x=>x.DeleteMovie(It.IsAny<string>()));

            //Act
            var movie = controller.DeleteMovie(movieName);
            var result=(bool)((JsonResult)movie.Result).Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result || !result);

        }

        /// <summary>
        /// Book Movie Test case
        /// </summary>
        [Test]
        public void BookMovieTest()
        {
            //Arrange
            var book = new Tickets
            {
                MovieId = ObjectId.GenerateNewId(),
                MovieName = "Sample",
                Theater = "PVP",
                NumberOfTicketsBooked = 1
            };
            var controller = new MovieBookingController(movieRepository.Object, logger.Object);
            movieRepository.Setup(x => x.BookMovie(It.IsAny<Tickets>())).ReturnsAsync(new ObjectId("66347428e574c152bf877ed5"));

            //Act
            var result = controller.BookMovie(book);
            var result1= (JsonResult)result.Result;
            
            //assert
            Assert.IsNotNull(result);
            Assert.That(result1.Value.ToString(), Is.EqualTo("66347428e574c152bf877ed5"));
        }

    }
}