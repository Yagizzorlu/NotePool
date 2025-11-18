using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Queries.User.GetMyProfile
{
    public class GetMyProfileQueryRequest : IRequest<GetMyProfileQueryResponse>
    {
    }
}
