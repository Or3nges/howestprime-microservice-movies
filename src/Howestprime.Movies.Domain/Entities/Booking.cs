using System;
using System.Collections.Generic;
using Howestprime.Movies.Domain.Enums;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Domain.Entities
{
    public sealed class BookingId : UuidEntityId
    {
        public BookingId(string? id = "") : base(id)
        {
        }
    }

    public class Booking : Entity<BookingId>
    {
        public int StandardVisitors { get; private set; }
        public int DiscountVisitors { get; private set; }
        public BookingStatus Status { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public List<int> SeatNumbers { get; private set; }
        public string RoomName { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Booking(
            BookingId id,
            int standardVisitors,
            int discountVisitors,
            BookingStatus status,
            PaymentStatus paymentStatus,
            List<int> seatNumbers,
            string roomName,
            DateTime createdAt)
            : base(id)
        {
            StandardVisitors = standardVisitors;
            DiscountVisitors = discountVisitors;
            Status = status;
            PaymentStatus = paymentStatus;
            SeatNumbers = seatNumbers;
            RoomName = roomName;
            CreatedAt = createdAt;
        }

        public override void ValidateState()
        {
            if (StandardVisitors < 0)
                throw new InvalidOperationException("Standard visitors cannot be negative.");
            if (DiscountVisitors < 0)
                throw new InvalidOperationException("Discount visitors cannot be negative.");
            if (string.IsNullOrWhiteSpace(RoomName))
                throw new InvalidOperationException("Room name cannot be null or empty.");
            if (SeatNumbers == null || SeatNumbers.Count == 0)
                throw new InvalidOperationException("Seat numbers cannot be null or empty.");
        }
    }
}
