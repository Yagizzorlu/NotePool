using MediatR;
using NotePool.Application.Features.Commands.Department.UpdateDepartment;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Department.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommandRequest, UpdateDepartmentCommandResponse>
    {
        IDepartmentReadRepository _departmentReadRepository;
        IDepartmentWriteRepository _departmentWriteRepository;

        public UpdateDepartmentCommandHandler(IDepartmentReadRepository departmentReadRepository, IDepartmentWriteRepository departmentWriteRepository)
        {
            _departmentReadRepository = departmentReadRepository;
            _departmentWriteRepository = departmentWriteRepository;
        }

        public async Task<UpdateDepartmentCommandResponse> Handle(UpdateDepartmentCommandRequest request, CancellationToken cancellationToken)
        {
            D.Department department = await _departmentReadRepository.GetByIdAsync(request.Id);
            department.Name = request.Name;
            department.Code = request.Code;
            department.InstitutionId = Guid.Parse(request.InstitutionId);
            _departmentWriteRepository.Update(department);
            await _departmentWriteRepository.SaveAsync();
            return new();
        }
    }
}
