using System;
using System.Threading.Tasks;
using Xunit;
using Howestprime.Movies.Application;
using Howestprime.Movies.Application.Movies.RegisterMovie;
using Howestprime.Movies.Domain.Movie;
using UnitTests.Shared;
using Domaincrafters.Domain;
using Howestprime.Movies.Domain.Events;
using System.Collections.Generic;

namespace UnitTests.Application
{
    public class TestEventSubscriber : IDomainEventSubscriber
    {
        public List<IDomainEvent> HandledEvents { get; } = new List<IDomainEvent>();
        private readonly Type _targetEventType;

        public TestEventSubscriber(Type targetEventType)
        {
            _targetEventType = targetEventType;
        }

        public void HandleEvent(IDomainEvent domainEvent)
        {
            if (IsSubscribedTo(domainEvent))
            {
                HandledEvents.Add(domainEvent);
            }
        }

        public bool IsSubscribedTo(IDomainEvent domainEvent)
        {
            return domainEvent.GetType() == _targetEventType;
        }
    }

    public class RegisterMovieUseCaseTests
    {
        private readonly FakeMovieRepository _movieRepository;
        private readonly RegisterMovieUseCase _useCase;

        public RegisterMovieUseCaseTests()
        {
            _movieRepository = new FakeMovieRepository();
            _useCase = new RegisterMovieUseCase(_movieRepository);
        }

        [Fact]
        public async Task ExecuteAsync_WithValidCommand_RegistersMovie()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = "Inception",
                Description = "A mind-bending thriller",
                Genre = "Sci-Fi",
                Actors = "Leonardo DiCaprio",
                AgeRating = 12,
                Duration = 148,
                PosterUrl = "https://example.com/inception.jpg",
                Year = 2010
            };

            // Act
            var result = await _useCase.ExecuteAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Genre, result.Genre);
            Assert.Equal(command.Actors, result.Actors);
            Assert.Equal(command.AgeRating.ToString(), result.AgeRating);
            Assert.Equal(command.Duration, result.Duration);
            Assert.Equal(command.PosterUrl, result.PosterUrl);
            Assert.Equal(command.Year, result.Year);

            // Verify movie was saved
            var savedMovie = await _movieRepository.GetByIdAsync(new MovieId(result.Id.Value));
            Assert.NotNull(savedMovie);
            Assert.Equal(result.Id.Value, savedMovie.Id.Value);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullTitle_ThrowsArgumentException()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = null,
                Duration = 120
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_WithEmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = "",
                Duration = 120
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_WithZeroDuration_ThrowsArgumentException()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = "Test Movie",
                Duration = 0
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_WithNegativeDuration_ThrowsArgumentException()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = "Test Movie",
                Duration = -1
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecuteAsync(command));
        }

        [Fact]
        public async Task ExecuteAsync_PublishesDomainEvent()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = "Inception",
                Description = "A mind-bending thriller",
                Genre = "Sci-Fi",
                Actors = "Leonardo DiCaprio",
                AgeRating = 12,
                Duration = 148,
                PosterUrl = "https://example.com/inception.jpg",
                Year = 2010
            };

            var subscriber = new TestEventSubscriber(typeof(MovieRegistered));
            DomainEventPublisher.Instance.AddDomainEventSubscriber(subscriber);

            // Act
            var movie = await _useCase.ExecuteAsync(command);

            // Assert
            Assert.Single(subscriber.HandledEvents);
            var evt = Assert.IsType<MovieRegistered>(subscriber.HandledEvents[0]);
            Assert.Equal(movie.Id.Value, evt.MovieId);
            Assert.Equal(command.Title, evt.Title);
            Assert.Equal(command.Year, evt.Year);
            Assert.Equal(command.Duration, evt.Duration);
            Assert.Equal(command.Genre, evt.Genre);
            Assert.Equal(command.Actors, evt.Actors);
            Assert.Equal(command.AgeRating, evt.AgeRating);
            Assert.Equal(command.PosterUrl, evt.PosterUrl);
        }

        [Fact]
        public async Task ExecuteAsync_WithNullOptionalFields_CreatesMovieWithDefaults()
        {
            // Arrange
            var command = new RegisterMovieCommand
            {
                Title = "Test Movie",
                Duration = 120,
                Year = 2024
            };

            // Act
            var result = await _useCase.ExecuteAsync(command);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Movie", result.Title);
            Assert.Equal(string.Empty, result.Description);
            Assert.Equal(string.Empty, result.Genre);
            Assert.Equal(string.Empty, result.Actors);
            Assert.Equal("0", result.AgeRating);
            Assert.Equal(120, result.Duration);
            Assert.Equal(string.Empty, result.PosterUrl);
            Assert.Equal(2024, result.Year);
        }
    }
}
