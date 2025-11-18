using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Abstractions.Hubs
{
    public interface IDepartmentHubService
    {
        Task DepartmentCreatedMessageAsync(string message);
        Task DepartmentUpdatedMessageAsync(string message);
    }
}
