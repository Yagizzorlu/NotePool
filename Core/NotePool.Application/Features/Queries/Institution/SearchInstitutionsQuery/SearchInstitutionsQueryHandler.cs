using MediatR;
using Microsoft.EntityFrameworkCore;
using NotePool.Application.Features.Queries.Institution.SearchInstitutionsQuery;
using NotePool.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.Institution.Search
{
    public class SearchInstitutionsQueryHandler : IRequestHandler<SearchInstitutionsQueryRequest, SearchInstitutionsQueryResponse>
    {
        private readonly IInstitutionReadRepository _institutionReadRepository;

        public SearchInstitutionsQueryHandler(IInstitutionReadRepository institutionReadRepository)
        {
            _institutionReadRepository = institutionReadRepository;
        }

        public async Task<SearchInstitutionsQueryResponse> Handle(SearchInstitutionsQueryRequest request, CancellationToken cancellationToken)
        {
            var query = _institutionReadRepository.GetAll(false);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query
                    .Where(i =>
                        i.Name.Contains(request.SearchTerm) ||
                        (i.City != null && i.City.Contains(request.SearchTerm))
                    );
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var institutionList = await query
                .OrderBy(i => i.Name)
                .Take(request.PageSize)
                .Select(institution => new
                {
                    institution.Id,
                    institution.Name,
                    institution.City
                })
                .ToListAsync(cancellationToken);

            return new SearchInstitutionsQueryResponse
            {
                Institutions = institutionList,
                TotalCount = totalCount
            };
        }
    }
}
