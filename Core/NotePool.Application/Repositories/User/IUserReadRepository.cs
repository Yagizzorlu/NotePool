using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Repositories
{
    public interface IUserReadRepository
    {
        IQueryable<User> GetAll(bool tracking = true);
        IQueryable<User> GetWhere(Expression<Func<User, bool>> method, bool tracking = true);
        Task<User> GetSingleAsync(Expression<Func<User, bool>> method, bool tracking = true);
        Task<User> GetByIdAsync(string id, bool tracking = true);
    }
}
