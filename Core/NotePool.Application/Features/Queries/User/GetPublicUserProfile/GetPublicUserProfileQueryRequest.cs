using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetPublicUserProfile
{
    public class GetPublicUserProfileQueryRequest : IRequest<GetPublicUserProfileQueryResponse>
    {
        public string Id { get; set; }
    }
}
