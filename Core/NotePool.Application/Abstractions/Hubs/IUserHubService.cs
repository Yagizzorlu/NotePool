using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Abstractions.Hubs
{
    public interface IUserHubService
    {
        Task UserCreatedMessageAsync(string message);
        Task UserProfileUpdatedMessageAsync(string message);
        Task UserLoggedInMessageAsync(string message);
    }
}
