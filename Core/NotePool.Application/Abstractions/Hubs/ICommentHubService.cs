using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Abstractions.Hubs
{
    public interface ICommentHubService
    {
        Task CommentAddedMessageAsync(string message);
        Task CommentDeletedMessageAsync(string message);
        Task CommentUpdatedMessageAsync(string message);
    }
}
