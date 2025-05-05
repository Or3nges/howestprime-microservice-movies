using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Howestprime.Movies.Domain.Entities;

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
        }
    }
}
