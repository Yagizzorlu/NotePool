// Persistence/Repositories/UserWriteRepository.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using NotePool.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotePool.Persistence.Repositories
{
    public class UserWriteRepository : IUserWriteRepository
    {
        private readonly NotePoolDbContext _context;
        public UserWriteRepository(NotePoolDbContext context)
        {
            _context = context;
        }

        public DbSet<User> Table => _context.Users;

        public bool Remove(User model)
        {
            EntityEntry<User> entityEntry = Table.Remove(model);
            return entityEntry.State == EntityState.Deleted;
        }

        public async Task<bool> RemoveAsync(string id)
        {
            // 'string Id' mimarine %100 uyumlu
            User model = await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
            return Remove(model);
        }

        public bool RemoveRange(List<User> datas)
        {
            Table.RemoveRange(datas);
            return true;
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();


        public bool Update(User model)
        {
            EntityEntry entityEntry = Table.Update(model);
            return entityEntry.State == EntityState.Modified;
        }
    }
}
