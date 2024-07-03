using EskroAfrica.PaymentService.Domain.Entities;

namespace EskroAfrica.PaymentService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task SaveChangesAsync();
    }
}
