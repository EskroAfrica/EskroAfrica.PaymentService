using EskroAfrica.PaymentService.Application.Interfaces;
using EskroAfrica.PaymentService.Domain.Entities;
using EskroAfrica.PaymentService.Infrastructure.Data;

namespace EskroAfrica.PaymentService.Infrastructure.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PaymentServiceDbContext _dbContext;
        private Dictionary<string, object> Repositories { get; set; } = new Dictionary<string, object>();

        public UnitOfWork(PaymentServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (Repositories.ContainsKey(nameof(T))) return (GenericRepository<T>)Repositories[nameof(T)];

            Repositories.Add(nameof(T), new GenericRepository<T>(_dbContext));
            return (GenericRepository<T>)Repositories[nameof(T)];
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
