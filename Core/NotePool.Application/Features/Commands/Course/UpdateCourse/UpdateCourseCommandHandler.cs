using MediatR;
using NotePool.Application.Features.Commands.Course.UpdateCourse;
using NotePool.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = NotePool.Domain.Entities;

namespace NotePool.Application.Features.Commands.Course.UpdateCourse
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommandRequest, UpdateCourseCommandResponse>
    {
        ICourseReadRepository _courseReadRepository;
        ICourseWriteRepository _courseWriteRepository;

        public UpdateCourseCommandHandler(ICourseReadRepository courseReadRepository, ICourseWriteRepository courseWriteRepository)
        {
            _courseReadRepository = courseReadRepository;
            _courseWriteRepository = courseWriteRepository;
        }

        public async Task<UpdateCourseCommandResponse> Handle(UpdateCourseCommandRequest request, CancellationToken cancellationToken)
        {
            C.Course course = await _courseReadRepository.GetByIdAsync(request.Id);
            course.Name = request.Name;
            course.Code = request.Code;
            course.Year = request.Year;
            course.DepartmentId = Guid.Parse(request.DepartmentId);
            _courseWriteRepository.Update(course);
            await _courseWriteRepository.SaveAsync();
            return new();
        }
    }
}

