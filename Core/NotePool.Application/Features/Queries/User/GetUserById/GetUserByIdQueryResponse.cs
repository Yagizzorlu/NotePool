using System;

namespace NotePool.Application.Features.Queries.User.GetUserById
{
    public class GetUserByIdQueryResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? ProfileImage { get; set; }

        public Guid InstitutionId { get; set; }
        public Guid DepartmentId { get; set; }

        public string InstitutionName { get; set; }
        public string DepartmentName { get; set; }

        public int NoteCount { get; set; }
        public int CommentCount { get; set; }
        public int ReplyCount { get; set; }
        public int LikeCount { get; set; }
        public int ReactionsCount { get; set; }
        public int BookmarksCount { get; set; }
        public int DownloadsCount { get; set; }
    }
}
