using MediatR;
using NotePool.Application.Features.Commands.Course.AddCourse;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Course.AddCourse
{
    public class AddCourseCommandHandler : IRequestHandler<AddCourseCommandRequest, AddCourseCommandResponse>
    {
        private readonly ICourseWriteRepository _courseWriteRepository;

        public AddCourseCommandHandler(ICourseWriteRepository courseWriteRepository)
        {
            _courseWriteRepository = courseWriteRepository;
        }

        public async Task<AddCourseCommandResponse> Handle(AddCourseCommandRequest request, CancellationToken cancellationToken)
        {
            C.Course course = new()
            {
                Name = request.Name,
                Code = request.Code,
                Year = request.Year,
                DepartmentId = request.DepartmentId
            };

            await _courseWriteRepository.AddAsync(course);
            await _courseWriteRepository.SaveAsync();

            return new AddCourseCommandResponse
            {
                Success = true,
                Message = "Başarıyla Yüklendi",
                Id = course.Id
            };
        }
    }
}
