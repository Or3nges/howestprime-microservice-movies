using System;
using System.Linq;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Application.Movies.FindMovieEventsForMonth;
using Howestprime.Movies.Domain.Entities;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework;
using Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTests.IntegrationTests
{
    public class FindMovieEventsForMonthTests
    {
        private MoviesDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseNpgsql("Host=localhost;Port=40021;Database=movies-howestprime;Username=devuser;Password=devpassword")
                .Options;
            return new MoviesDbContext(options);
        }

        [Fact]
        public async Task ReturnsEventsForValidMonth()
        {
            var context = CreateDbContext();
            context.Database.EnsureCreated();
            var movieRepo = new MovieRepository(context);
            var roomRepo = new RoomRepository(context);
            await roomRepo.SeedRoomsAsync();
            var eventRepo = new MovieEventRepository(context);
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);

            var movie = new Movie(Guid.NewGuid(), "MonthlyTest", "desc", "genre", "actors", "12", 120, "poster");
            await movieRepo.AddAsync(movie);
            var room = context.Rooms.First();

            var movieEvent = new MovieEvent
            {
                Id = Guid.NewGuid(),
                MovieId = movie.Id,
                RoomId = room.Id,
                Date = new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc),
                Time = new TimeSpan(15, 0, 0),
                Capacity = 50
            };
            await eventRepo.AddAsync(movieEvent);

            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 5 };
            var result = await useCase.ExecuteAsync(query);

            Assert.NotEmpty(result);
            Assert.Contains(result, e => e.Id == movieEvent.Id);
        }

        [Fact]
        public async Task ReturnsEmptyListForMonthWithNoEvents()
        {
            var context = CreateDbContext();
            var movieRepo = new MovieRepository(context);
            var roomRepo = new RoomRepository(context);
            await roomRepo.SeedRoomsAsync();
            var eventRepo = new MovieEventRepository(context);
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);

            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 6 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result);
        }

        [Fact]
        public async Task ReturnsEmptyListForInvalidMonth()
        {
            var context = CreateDbContext();
            var movieRepo = new MovieRepository(context);
            var roomRepo = new RoomRepository(context);
            await roomRepo.SeedRoomsAsync();
            var eventRepo = new MovieEventRepository(context);
            var useCase = new FindMovieEventsForMonthUseCase(movieRepo, eventRepo, roomRepo);

            var query = new FindMovieEventsForMonthQuery { Year = 2025, Month = 0 };
            var result = await useCase.ExecuteAsync(query);
            Assert.Empty(result);
        }
    }
}
