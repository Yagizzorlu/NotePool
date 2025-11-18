using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetMyProfile
{
    public class GetMyProfileQueryResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? ProfileImage { get; set; }
        public Guid InstitutionId { get; set; }
        public Guid DepartmentId { get; set; }
        public string? InstitutionName { get; set; }
        public string? DepartmentName { get; set; }

        public int NotesCount { get; set; }
        public int CommentsCount { get; set; }
        public int BookmarksCount { get; set; }
        public int ReactionsCount { get;  set; }
        public int RepliesCount { get;  set; }
        public int DownloadsCount { get; set; }
    }
}
