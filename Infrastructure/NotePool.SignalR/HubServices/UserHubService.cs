using Microsoft.AspNetCore.SignalR;
using NotePool.Application.Abstractions.Hubs;
using NotePool.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.SignalR.HubServices
{
    public class UserHubService : IUserHubService
    {
        readonly IHubContext<UserHub> _hubContext;
        public UserHubService(IHubContext<UserHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task UserCreatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.UserCreatedMessage, message);
        }

        public async Task UserLoggedInMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.UserLoggedInMessage, message);
        }

        public async Task UserProfileUpdatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.UserProfileUpdatedMessage, message);
        }
    }
}
