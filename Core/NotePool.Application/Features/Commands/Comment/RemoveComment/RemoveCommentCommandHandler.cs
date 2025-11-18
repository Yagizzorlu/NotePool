using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Commands.Comment.RemoveComment;
using NotePool.Application.Abstractions.Hubs;

namespace NotePool.Application.Features.Commands.Comment.RemoveComment
{
    public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommandRequest, RemoveCommentCommandResponse>
    {
        private readonly ICommentReadRepository _commentReadRepository;
        private readonly ICommentWriteRepository _commentWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly ICommentHubService _commentHubService;

        public RemoveCommentCommandHandler(
            ICommentReadRepository commentReadRepository,
            ICommentWriteRepository commentWriteRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<Domain.Entities.User> userManager,
            ICommentHubService commentHubService)
        {
            _commentReadRepository = commentReadRepository;
            _commentWriteRepository = commentWriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _commentHubService = commentHubService;
        }

        public async Task<RemoveCommentCommandResponse> Handle(RemoveCommentCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var loggedInUser = await _userManager.FindByIdAsync(loggedInUserIdString);
            if (loggedInUser == null)
                throw new Exception("Giriş yapan kullanıcı bulunamadı.");

            var comment = await _commentReadRepository.GetByIdAsync(request.Id);
            if (comment == null)
                throw new Exception("Silinecek yorum bulunamadı.");

            var userIsAdmin = await _userManager.IsInRoleAsync(loggedInUser, "Admin");

            if (comment.UserId.ToString() != loggedInUserIdString && !userIsAdmin)
            {
                throw new Exception("Bu yorumu silmeye yetkiniz yok.");
            }

            _commentWriteRepository.Remove(comment);
            await _commentWriteRepository.SaveAsync();
            await _commentHubService.CommentDeletedMessageAsync($"Yorum silindi.");

            return new RemoveCommentCommandResponse
            {
                Succeeded = true,
                Message = "Yorum başarıyla silindi."
            };
        }
    }
}
