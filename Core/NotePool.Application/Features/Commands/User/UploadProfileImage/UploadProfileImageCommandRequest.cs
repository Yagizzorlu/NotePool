using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.User.UploadProfileImage
{
    public class UploadProfileImageCommandRequest : IRequest<UploadProfileImageCommandResponse>
    {
        public IFormFile File { get; set; }
    }
}

