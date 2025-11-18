using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.SignalR
{
    public static class ReceiveFunctionNames
    {
        public const string NoteCreatedMessage = "NoteCreatedMessage";
        public const string NoteDeletedMessage = "NoteDeletedMessage";
        public const string NoteUpdatedMessage = "NoteUpdatedMessage";
        public const string NoteDownloadedMessage = "NoteDownloadedMessage";

        public const string CommentAddedMessage = "CommentAddedMessage";
        public const string CommentDeletedMessage = "CommentDeletedMessage";
        public const string CommentUpdatedMessage = "CommentUpdatedMessage";

        public const string ReactionAddedMessage = "ReactionAddedMessage";
        public const string ReactionDeletedMessage = "ReactionDeletedMessage";

        public const string BookmarkAddedMessage = "BookmarkAddedMessage";
        public const string BookmarkRemovedMessage = "BookmarkRemovedMessage";

        public const string CourseCreatedMessage = "CourseCreatedMessage";
        public const string CourseUpdatedMessage = "CourseUpdatedMessage";

        public const string DepartmentCreatedMessage = "DepartmentCreatedMessage";
        public const string DepartmentUpdatedMessage = "DepartmentUpdatedMessage";

        public const string InstitutionCreatedMessage = "InstitutionCreatedMessage";
        public const string InstitutionUpdatedMessage = "InstitutionUpdatedMessage";

        public const string UserCreatedMessage = "UserCreatedMessage";
        public const string UserProfileUpdatedMessage = "UserProfileUpdatedMessage";
        public const string UserLoggedInMessage = "UserLoggedInMessage";
    }
}
