using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Entities;

namespace Howestprime.Movies.Domain.Shared
{
    public class MovieEvent
    {
        public MovieEventId Id { get; private set; }
        public MovieId MovieId { get; private set; }
        public RoomId RoomId { get; private set; }
        public DateOnly Date { get; private set; }
        public TimeOnly Time { get; private set; }
        public int Visitors { get; private set; }
        public List<BookingId> Bookings { get; private set; }

        public MovieEvent(MovieEventId id, MovieId movieId, RoomId roomId, DateOnly date, TimeOnly time, int visitors, List<BookingId> bookings)
        {
            Id = id;
            MovieId = movieId;
            RoomId = roomId;
            Date = date;
            Time = time;
            Visitors = visitors;
            Bookings = bookings ?? new List<BookingId>();
        }
    }
}