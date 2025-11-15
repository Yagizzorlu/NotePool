using MediatR;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Department.AddDepartment
{
    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommandRequest, AddDepartmentCommandResponse>
    {
        private readonly IDepartmentWriteRepository _departmentWriteRepository;

        public AddDepartmentCommandHandler(IDepartmentWriteRepository departmentWriteRepository)
        {
            _departmentWriteRepository = departmentWriteRepository;
        }

        public async Task<AddDepartmentCommandResponse> Handle(AddDepartmentCommandRequest request, CancellationToken cancellationToken)
        {
            D.Department department = new()
            {
                Name = request.Name,
                Code = request.Code,
                InstitutionId = request.InstitutionId
            };

            await _departmentWriteRepository.AddAsync(department);
            await _departmentWriteRepository.SaveAsync();

            return new AddDepartmentCommandResponse
            {
                Success = true,
                Message = "Başarıyla Yüklendi",
                Id = department.Id
            };
        }
    }
}
