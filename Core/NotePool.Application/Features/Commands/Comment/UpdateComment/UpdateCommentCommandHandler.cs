using MediatR;
using Microsoft.AspNetCore.Http;
using NotePool.Application.Abstractions.Hubs;
using NotePool.Application.Features.Commands.Comment.UpdateComment;
using NotePool.Application.Repositories;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Comment.UpdateComment
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommandRequest, UpdateCommentCommandResponse>
    {
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentHubService _commentHubService;

        public UpdateCommentCommandHandler(
            ICommentReadRepository commentReadRepository,
            ICommentWriteRepository commentWriteRepository,
            IHttpContextAccessor httpContextAccessor,
            ICommentHubService commentHubService)
        {
            _commentReadRepository = commentReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _commentHubService = commentHubService;
        }

        public async Task<UpdateCommentCommandResponse> Handle(UpdateCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var comment = await _commentReadRepository.GetByIdAsync(request.Id);

            if (comment == null)
                throw new Exception("Güncellenecek yorum bulunamadı.");

            if (comment.UserId.ToString() != loggedInUserIdString)
            {
                throw new Exception("Bu yorumu düzenlemeye yetkiniz yok.");
            }

            comment.Content = request.Content;

            _commentWriteRepository.Update(comment);
            await _commentWriteRepository.SaveAsync();
            await _commentHubService.CommentUpdatedMessageAsync($"Yorum güncellendi.");

            return new UpdateCommentCommandResponse();
        }
    }
}
