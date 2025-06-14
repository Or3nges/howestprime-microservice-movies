using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Domain.Shared;
using Howestprime.Movies.Domain.Movie;
using Howestprime.Movies.Domain.MovieEvent;
using Howestprime.Movies.Domain.Room;

namespace Howestprime.Movies.Application.Movies.FindMovieEventsForMonth
{
    public class FindMovieEventsForMonthUseCase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieEventRepository _movieEventRepository;
        private readonly IRoomRepository _roomRepository;

        public FindMovieEventsForMonthUseCase(
            IMovieRepository movieRepository,
            IMovieEventRepository movieEventRepository,
            IRoomRepository roomRepository)
        {
            _movieRepository = movieRepository;
            _movieEventRepository = movieEventRepository;
            _roomRepository = roomRepository;
        }

        public async Task<List<MovieEventData>> ExecuteAsync(FindMovieEventsForMonthQuery query)
        {

            var startDate = new DateTime(query.Year, query.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endDate = startDate.AddMonths(1).AddTicks(-1); // Last moment of the last day of the month


            var events = await _movieEventRepository.GetEventsInRangeAsync(startDate, endDate);
            var eventDatas = new List<MovieEventData>();

            foreach (var evt in events)
            {
                var movie = await _movieRepository.GetByIdAsync(evt.MovieId);
                if (movie == null) continue;

                var room = await _roomRepository.GetByIdAsync(evt.RoomId);
                if (room == null) continue;

                eventDatas.Add(new MovieEventData
                {
                    Id = evt.Id.Value,
                    Time = evt.Time,
                    MovieId = evt.MovieId.Value,
                    Capacity = evt.Capacity,
                    Room = new RoomData
                    {
                        Id = room.Id.Value,
                        Name = room.Name,
                        Capacity = room.Capacity
                    }
                });
            }

            return eventDatas;
        }
    }

    public class ScheduledMovieEventsResponse
    {
        public List<ExtendedMovieEventData> Data { get; set; } = new List<ExtendedMovieEventData>();
    }
}
