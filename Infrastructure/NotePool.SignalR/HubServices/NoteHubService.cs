using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NotePool.Application.Abstractions.Hubs;
using NotePool.SignalR.Hubs;

namespace NotePool.SignalR.HubServices
{
    public class NoteHubService : INoteHubService
    {
        readonly IHubContext<NoteHub> _hubContext;

        public NoteHubService(IHubContext<NoteHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NoteCreatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.NoteCreatedMessage, message);
        }

        public async Task NoteDeletedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.NoteDeletedMessage, message);
        }

        public async Task NoteDownloadedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.NoteDownloadedMessage, message);
        }

        public async Task NoteUpdatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.NoteUpdatedMessage, message);
        }
    }
}
