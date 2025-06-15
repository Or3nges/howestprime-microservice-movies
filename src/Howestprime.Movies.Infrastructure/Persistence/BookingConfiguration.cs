using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Howestprime.Movies.Domain.Booking;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;


namespace Howestprime.Movies.Infrastructure.Persistence
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.StandardVisitors).IsRequired();
            builder.Property(b => b.DiscountVisitors).IsRequired();
            builder.Property(b => b.Status).IsRequired();
            builder.Property(b => b.PaymentStatus).IsRequired();
            builder.Property(b => b.RoomName).IsRequired();
            builder.Property(b => b.CreatedAt).IsRequired();
            builder.Property(b => b.SeatNumbers)
                .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', System.StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList()
                );

            var valueComparer = new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            builder.Property(b => b.SeatNumbers).Metadata.SetValueComparer(valueComparer);
        }
    }
}