using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Abstractions.Hubs
{
    public interface INoteHubService
    {
        Task NoteCreatedMessageAsync(string message);
        Task NoteDeletedMessageAsync(string message);
        Task NoteUpdatedMessageAsync(string message);
        Task NoteDownloadedMessageAsync(string message);
    }
}
