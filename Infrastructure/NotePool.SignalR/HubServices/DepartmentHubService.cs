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
    public class DepartmentHubService : IDepartmentHubService
    {
        readonly IHubContext<DepartmentHub> _hubContext;
        public DepartmentHubService(IHubContext<DepartmentHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task DepartmentCreatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.DepartmentCreatedMessage, message);
        }

        public async Task DepartmentUpdatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.DepartmentUpdatedMessage, message);
        }
    }
}
