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
    public class CourseHubService : ICourseHubService
    {
        readonly IHubContext<CourseHub> _hubContext;
        public CourseHubService(IHubContext<CourseHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task CourseCreatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.CourseCreatedMessage, message);
        }

        public async Task CourseUpdatedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(ReceiveFunctionNames.CourseUpdatedMessage, message);
        }
    }
}
