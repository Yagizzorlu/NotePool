using Microsoft.AspNetCore.Builder;
using NotePool.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<NoteHub>("/notes-hub");
            webApplication.MapHub<CommentHub>("/comments-hub");
            webApplication.MapHub<ReactionHub>("/reactions-hub");
            webApplication.MapHub<BookmarkHub>("/bookmarks-hub");
            webApplication.MapHub<CourseHub>("/courses-hub");
            webApplication.MapHub<DepartmentHub>("/departments-hub");
            webApplication.MapHub<InstitutionHub>("/institutions-hub");
            webApplication.MapHub<UserHub>("/users-hub");
        }
    }
}
