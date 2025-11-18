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
    public class ReactionHubService : IReactionHubService
    {
        readonly IHubContext<ReactionHub> _hubContext;
        public ReactionHubService(IHubContext<ReactionHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task ReactionAddedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ReactionAddedMessage, message);
        }

        public async Task ReactionDeletedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.ReactionDeletedMessage, message);
        }
    }
}
