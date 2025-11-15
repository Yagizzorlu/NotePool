using NotePool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Repositories
{
    public interface IUserWriteRepository
    {
        bool Remove(User model);
        bool RemoveRange(List<User> datas);
        Task<bool> RemoveAsync(string id);
        bool Update(User model);
        Task<int> SaveAsync();
    }
}
