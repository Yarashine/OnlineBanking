using AccountService.DAL.Contracts.Repositories;
using AccountService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.DAL.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    public IQueryable<T> ApplyPagination(IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}