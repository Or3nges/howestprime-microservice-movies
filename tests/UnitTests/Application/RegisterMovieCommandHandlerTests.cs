using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Domain.Events;
using Howestprime.Movies.Domain.Movie;
using UnitTests.Shared;
using Xunit;
using Domaincrafters.Domain;

namespace UnitTests.Application
{
    public class RegisterMovieCommandHandlerTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly RegisterMovieCommandHandler _handler;
        private readonly FakeDomainEventSubscriber _subscriber;

        public RegisterMovieCommandHandlerTests()
        {
            _movieRepository = new FakeMovieRepository();
            _handler = new RegisterMovieCommandHandler(_movieRepository);
            _subscriber = new FakeDomainEventSubscriber();
            DomainEventPublisher.Instance.AddDomainEventSubscriber(_subscriber);
        }

        [Fact]
        public async Task Handle_ShouldCreateAndSaveMovie_AndPublishEvent()
        {
            // Arrange
            _subscriber.HandledEvents.Clear();
            var command = new RegisterMovieCommand
            {
                Id = new MovieId(),
                Title = "Test Movie",
                Description = "A great movie",
                Year = 2024,
                Genre = "Action",
                Actors = "John Doe",
                AgeRating = 12,
                Duration = 120,
                PosterUrl = "http://example.com/poster.jpg"
            };

            // Act
            var result = await _handler.Handle(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Id, result.Id);

            var dictionary = (IDictionary<MovieId, Movie>)_movieRepository.Movies;
            var savedMovie = dictionary.Values.FirstOrDefault(m => m.Id == result.Id);
            Assert.NotNull(savedMovie);
            Assert.Equal(command.Title, savedMovie.Title);

            var publishedEvent = _subscriber.HandledEvents.OfType<MovieRegistered>().FirstOrDefault();
            Assert.NotNull(publishedEvent);
            Assert.NotNull(publishedEvent.MovieId);
        }
    }
}
