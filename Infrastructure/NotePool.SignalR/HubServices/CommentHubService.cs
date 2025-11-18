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
    public class CommentHubService : ICommentHubService
    {
        readonly IHubContext<CommentHub> _hubContext;
        public CommentHubService(IHubContext<CommentHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task CommentAddedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.CommentAddedMessage, message);
        }

        public async Task CommentDeletedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.CommentDeletedMessage, message);
        }

        public async Task CommentUpdatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.CommentUpdatedMessage, message);
        }
    }
}
