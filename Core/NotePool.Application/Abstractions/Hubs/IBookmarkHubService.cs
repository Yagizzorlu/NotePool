using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Abstractions.Hubs
{
    public interface IBookmarkHubService
    {
        Task BookmarkAddedMessageAsync(string message);
        Task BookmarkRemovedMessageAsync(string message);
    }
}
