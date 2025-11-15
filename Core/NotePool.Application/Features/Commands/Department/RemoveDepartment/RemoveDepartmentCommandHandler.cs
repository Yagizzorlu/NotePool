using MediatR;
using NotePool.Application.Features.Commands.Department.RemoveDepartment;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotePool.Application.Features.Commands.Department.RemoveDepartment
{
    public class RemoveDepartmentCommandHandler : IRequestHandler<RemoveDepartmentCommandRequest, RemoveDepartmentCommandResponse>
    {
        private readonly IDepartmentWriteRepository _departmentWriteRepository;

        public RemoveDepartmentCommandHandler(IDepartmentWriteRepository departmentWriteRepository)
        {
            _departmentWriteRepository = departmentWriteRepository;
        }

        public async Task<RemoveDepartmentCommandResponse> Handle(RemoveDepartmentCommandRequest request, CancellationToken cancellationToken)
        {
            await _departmentWriteRepository.RemoveAsync(request.Id);
            await _departmentWriteRepository.SaveAsync();
            return new();
        }
    }
}
