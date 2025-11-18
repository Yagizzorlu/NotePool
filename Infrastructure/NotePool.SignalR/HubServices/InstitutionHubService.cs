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
    public class InstitutionHubService : IInstitutionHubService
    {
        readonly IHubContext<InstitutionHub> _hubContext;
        public InstitutionHubService(IHubContext<InstitutionHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task InstitutionCreatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.InstitutionCreatedMessage, message);
        }

        public async Task InstitutionUpdatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.InstitutionUpdatedMessage, message);
        }
    }
}
