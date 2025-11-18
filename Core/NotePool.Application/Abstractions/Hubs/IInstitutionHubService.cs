using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Abstractions.Hubs
{
    public interface IInstitutionHubService
    {
        Task InstitutionCreatedMessageAsync(string message);
        Task InstitutionUpdatedMessageAsync(string message);
    }
}
