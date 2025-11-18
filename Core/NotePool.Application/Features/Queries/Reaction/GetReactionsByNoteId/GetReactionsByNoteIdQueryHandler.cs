using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using NotePool.Application.Features.Queries.Reaction.GetReactionsByNoteId;
using NotePool.Domain.Entities;

namespace NotePool.Application.Features.Queries.Reaction.GetReactionsByNoteId
{
    public class GetReactionsByNoteIdQueryHandler : IRequestHandler<GetReactionsByNoteIdQueryRequest, GetReactionsByNoteIdQueryResponse>
    {
        private readonly IReactionReadRepository _reactionReadRepository;

        public GetReactionsByNoteIdQueryHandler(IReactionReadRepository reactionReadRepository)
        {
            _reactionReadRepository = reactionReadRepository;
        }

        public async Task<GetReactionsByNoteIdQueryResponse> Handle(GetReactionsByNoteIdQueryRequest request, CancellationToken cancellationToken)
        {
            var noteGuid = Guid.Parse(request.NoteId);

            var query = _reactionReadRepository.GetAll(false)
                .Where(r => r.NoteId == noteGuid && r.Type == request.Type);

            var totalCount = await query.CountAsync(cancellationToken);

            var userList = await query
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedDate)
                .Skip(request.Page * request.Size)
                .Take(request.Size)
                .Select(r => new
                {
                    Id = r.User != null ? r.User.Id : Guid.Empty,
                    UserName = r.User != null ? r.User.UserName : "Silinmiş Kullanıcı",
                    FirstName = r.User != null ? r.User.FirstName : "",
                    LastName = r.User != null ? r.User.LastName : "",
                    ProfileImage = r.User != null ? r.User.ProfileImage : null
                })
                .ToListAsync(cancellationToken);

            return new GetReactionsByNoteIdQueryResponse
            {
                Users = userList,
                TotalCount = totalCount
            };
        }
    }
}
