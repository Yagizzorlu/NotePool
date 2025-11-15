// Persistence/Repositories/UserReadRepository.cs
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using NotePool.Persistence.Contexts;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotePool.Persistence.Repositories
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly NotePoolDbContext _context;
        public UserReadRepository(NotePoolDbContext context)
        {
            _context = context;
        }

        public DbSet<User> Table => _context.Users;

        public IQueryable<User> GetAll(bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();
            return query;
        }

        public async Task<User> GetByIdAsync(string id, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
        }

        public async Task<User> GetSingleAsync(Expression<Func<User, bool>> method, bool tracking = true)
        {
            var query = Table.AsQueryable();
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync(method);
        }

        public IQueryable<User> GetWhere(Expression<Func<User, bool>> method, bool tracking = true)
        {
            var query = Table.Where(method);
            if (!tracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }
    }
}
