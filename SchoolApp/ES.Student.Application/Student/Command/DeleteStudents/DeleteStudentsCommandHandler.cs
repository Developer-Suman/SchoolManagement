using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ES.Student.Application.ServiceInterface;
using MediatR;
using TN.Shared.Domain.Abstractions;

namespace ES.Student.Application.Student.Command.DeleteStudents
{
    public  class DeleteStudentsCommandHandler:IRequestHandler<DeleteStudentsCommand,Result<bool>>
    {
        private readonly IStudentServices _studentServices;

        public DeleteStudentsCommandHandler(IStudentServices studentServices)
        {
            _studentServices = studentServices;


        }

        public async Task<Result<bool>> Handle(DeleteStudentsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var deleteStudent = await _studentServices.Delete(request.id, cancellationToken);
                if (deleteStudent is null)
                {
                    return Result<bool>.Failure("NotFound", "Student not Found");
                }
                return Result<bool>.Success(true);


            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting {request.id}", ex);
            }
        }
    }
}
