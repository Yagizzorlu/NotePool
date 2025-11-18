using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Abstractions.Hubs;
using NotePool.Application.Features.Commands.Reaction.SetReaction;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Reaction.SetReaction
{
    public class SetReactionCommandHandler : IRequestHandler<SetReactionCommandRequest, SetReactionCommandResponse>
    {
        private readonly IReactionReadRepository _reactionReadRepository;
        private readonly IReactionWriteRepository _reactionWriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SetReactionCommandHandler(
            IReactionReadRepository reactionReadRepository,
            IReactionWriteRepository reactionWriteRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _reactionReadRepository = reactionReadRepository;
            _reactionWriteRepository = reactionWriteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SetReactionCommandResponse> Handle(SetReactionCommandRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var loggedInUserId = Guid.Parse(loggedInUserIdString);
            var noteGuid = Guid.Parse(request.NoteId);
            var newReactionType = request.Type;
            ReactionType? userFinalReaction = null;

            var existingReaction = await _reactionReadRepository.GetSingleAsync(
                r => r.UserId == loggedInUserId && r.NoteId == noteGuid,
                true);

            if (existingReaction == null)
            {
                var newReaction = new Domain.Entities.Reaction
                {
                    Id = Guid.NewGuid(),
                    UserId = loggedInUserId,
                    NoteId = noteGuid,
                    Type = newReactionType
                };
                await _reactionWriteRepository.AddAsync(newReaction);
                userFinalReaction = newReactionType;
            }
            else
            {
                if (existingReaction.Type == newReactionType)
                {
                    _reactionWriteRepository.Remove(existingReaction);
                    userFinalReaction = null;
                }
                else
                {
                    existingReaction.Type = newReactionType;
                    _reactionWriteRepository.Update(existingReaction);
                    userFinalReaction = newReactionType;
                }
            }

            await _reactionWriteRepository.SaveAsync();

            var reactionsQuery = _reactionReadRepository.GetWhere(r => r.NoteId == noteGuid, false);

            var newLikeCount = await reactionsQuery.CountAsync(r => r.Type == ReactionType.Like, cancellationToken);
            var newDislikeCount = await reactionsQuery.CountAsync(r => r.Type == ReactionType.Dislike, cancellationToken);

            return new SetReactionCommandResponse
            {
                NewLikeCount = newLikeCount,
                NewDislikeCount = newDislikeCount,
                CurrentUserReaction = userFinalReaction
            };
        }
    }
}
