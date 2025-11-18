using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Abstractions.Hubs;
using NotePool.Application.Features.Commands.Comment.CreateComment;
using NotePool.Application.Repositories;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using C = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Comment.CreateComment
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommandRequest, CreateCommentCommandResponse>
    {
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        readonly ICommentHubService _commentHubService;

        public CreateCommentCommandHandler(
            ICommentWriteRepository commentWriteRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserReadRepository userReadRepository,
            ICommentHubService commentHubService)
        {
            _commentWriteRepository = commentWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _userReadRepository = userReadRepository;
            _commentHubService = commentHubService;
        }

        public async Task<CreateCommentCommandResponse> Handle(CreateCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var loggedInUserId = Guid.Parse(loggedInUserIdString);

            var user = await _userReadRepository.GetByIdAsync(loggedInUserIdString);
            if (user == null)
                throw new Exception("Yorum yapan kullanıcı profili bulunamadı.");

            C.Comment comment = new()
            {
                Id = Guid.NewGuid(),
                NoteId = request.NoteId,
                Content = request.Content,
                ParentId = request.ParentId,
                UserId = loggedInUserId
            };

            await _commentWriteRepository.AddAsync(comment);
            await _commentWriteRepository.SaveAsync();
            await _commentHubService.CommentAddedMessageAsync($"{request.Content} Yorumu Atılmıştır.");

            return new CreateCommentCommandResponse
            {
                Succeeded = true,
                CreatedComment = new
                {
                    comment.Id,
                    comment.Content,
                    comment.CreatedDate,
                    comment.ParentId,
                    CommentAuthorId = user.Id,
                    CommentAuthorName = user.UserName,
                    Replies = new List<object>()
                }
            };
        }
    }
}
