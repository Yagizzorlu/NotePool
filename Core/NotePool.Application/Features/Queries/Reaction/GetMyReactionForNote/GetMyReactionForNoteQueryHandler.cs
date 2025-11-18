using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using NotePool.Domain.Entities.Enums;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Queries.Reaction.GetMyReactionForNote;

namespace NotePool.Application.Features.Queries.Reaction.GetMyReactionForNote
{
    public class GetMyReactionForNoteQueryHandler : IRequestHandler<GetMyReactionForNoteQueryRequest, GetMyReactionForNoteQueryResponse>
    {
        private readonly IReactionReadRepository _reactionReadRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetMyReactionForNoteQueryHandler(
            IReactionReadRepository reactionReadRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _reactionReadRepository = reactionReadRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetMyReactionForNoteQueryResponse> Handle(GetMyReactionForNoteQueryRequest request, CancellationToken cancellationToken)
        {
            var loggedInUserIdString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(loggedInUserIdString))
                throw new Exception("Kullanıcı girişi bulunamadı.");

            var loggedInUserId = Guid.Parse(loggedInUserIdString);
            var noteGuid = Guid.Parse(request.NoteId);

            var existingReaction = await _reactionReadRepository.GetSingleAsync(
                r => r.UserId == loggedInUserId && r.NoteId == noteGuid,
                false); 

            if (existingReaction == null)
            {
                return new GetMyReactionForNoteQueryResponse
                {
                    UserReaction = null
                };
            }

            return new GetMyReactionForNoteQueryResponse
            {
                UserReaction = existingReaction.Type
            };
        }
    }
}
