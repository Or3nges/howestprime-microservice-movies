using System;
using System.Collections.Generic;
using System.Linq;
using Howestprime.Movies.Domain.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);
            
            builder.HasOne(b => b.MovieEvent)
                .WithMany(me => me.Bookings)
                .HasForeignKey(b => b.MovieEventId)
                .IsRequired();
            
            builder.Property(b => b.StandardVisitors).IsRequired();
            builder.Property(b => b.DiscountVisitors).IsRequired();
            builder.Property(b => b.Status).IsRequired();
            builder.Property(b => b.PaymentStatus).IsRequired();
            builder.Property(b => b.RoomName).IsRequired();
            builder.Property(b => b.CreatedAt).IsRequired();
            
            builder.Property(b => b.SeatNumbers)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(int.Parse)
                         .ToList(),
                    new ValueComparer<List<int>>(
                        (c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
                        c => c == null ? 0 : c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                        c => c == null ? new List<int>() : c.ToList()
                    ));
        }
    }
}
