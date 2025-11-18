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
    public class BookmarkHubService : IBookmarkHubService
    {
        readonly IHubContext<BookmarkHub> _hubContext;
        public BookmarkHubService(IHubContext<BookmarkHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task BookmarkAddedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.BookmarkAddedMessage, message);
        }

        public async Task BookmarkRemovedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.BookmarkRemovedMessage, message);
        }
    }
}
