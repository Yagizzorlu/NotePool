using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotePool.Application.Abstractions.Services;
using NotePool.Application.Abstractions.Services.Authentications;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using NotePool.Persistence.Contexts;
using NotePool.Persistence.Repositories;
using NotePool.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<NotePoolDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));

            services.AddIdentity<User, AppRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<NotePoolDbContext>();

            services.AddScoped<IBookmarkReadRepository, BookmarkReadRepository>();
            services.AddScoped<IBookmarkWriteRepository, BookmarkWriteRepository>();

            services.AddScoped<ICommentReadRepository, CommentReadRepository>();
            services.AddScoped<ICommentWriteRepository, CommentWriteRepository>();

            services.AddScoped<ICourseReadRepository, CourseReadRepository>();
            services.AddScoped<ICourseWriteRepository, CourseWriteRepository>();

            services.AddScoped<IInstitutionReadRepository, InstitutionReadRepository>();
            services.AddScoped<IInstitutionWriteRepository, InstitutionWriteRepository>();

            services.AddScoped<IDepartmentReadRepository, DepartmentReadRepository>();
            services.AddScoped<IDepartmentWriteRepository, DepartmentWriteRepository>();

            services.AddScoped<INoteReadRepository, NoteReadRepository>();
            services.AddScoped<INoteWriteRepository, NoteWriteRepository>();

            services.AddScoped<IReactionReadRepository, ReactionReadRepository>();
            services.AddScoped<IReactionWriteRepository, ReactionWriteRepository>();

            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<INoteDownloadReadRepository, NoteDownloadReadRepository>();
            services.AddScoped<INoteDownloadWriteRepository, NoteDownloadWriteRepository>();

            services.AddScoped<INotePdfFileReadRepository, NotePdfFileReadRepository>();
            services.AddScoped<INotePdfFileWriteRepository, NotePdfFileWriteRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExternalAuthentication, AuthService>();
            services.AddScoped<IInternalAuthentication, AuthService>();
        }
    }
}
