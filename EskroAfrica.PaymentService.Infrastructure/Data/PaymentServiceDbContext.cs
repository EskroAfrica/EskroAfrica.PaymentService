using EskroAfrica.PaymentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EskroAfrica.PaymentService.Infrastructure.Data
{
    public class PaymentServiceDbContext : DbContext
    {
        public DbSet<Payment> Payments { get; set; }

        public PaymentServiceDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
