using AccountService.DAL.Entities;

namespace AccountService.DAL.Contracts.Repositories;

public interface IRepository<T> where T : class
{
    public IQueryable<T> ApplyPagination(IQueryable<T> query, int pageNumber, int pageSize);
}