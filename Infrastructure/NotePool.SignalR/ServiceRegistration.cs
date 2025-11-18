using Microsoft.Extensions.DependencyInjection;
using NotePool.Application.Abstractions.Hubs;
using NotePool.SignalR.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection collection)
        {
            collection.AddTransient<INoteHubService, NoteHubService>();
            collection.AddTransient<ICommentHubService, CommentHubService>();
            collection.AddTransient<IReactionHubService, ReactionHubService>();
            collection.AddTransient<IBookmarkHubService, BookmarkHubService>();
            collection.AddTransient<ICourseHubService, CourseHubService>();
            collection.AddTransient<IDepartmentHubService, DepartmentHubService>();
            collection.AddTransient<IInstitutionHubService, InstitutionHubService>();
            collection.AddTransient<IUserHubService, UserHubService>();
            collection.AddSignalR();
        }
    }
}
