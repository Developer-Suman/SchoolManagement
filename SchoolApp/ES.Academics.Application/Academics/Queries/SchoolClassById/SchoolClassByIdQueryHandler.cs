using AutoMapper;
using ES.Academics.Application.ServiceInterface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Abstractions;

namespace ES.Academics.Application.Academics.Queries.SchoolClassById
{
    public class SchoolClassByIdQueryHandler : IRequestHandler<SchoolClassByIdQuery, Result<SchoolClassByIdResponse>>
    {
        private readonly ISchoolClassInterface _schoolClassInterface;
        private readonly IMapper _mapper;

        public SchoolClassByIdQueryHandler(ISchoolClassInterface schoolClassInterface, IMapper mapper)
        {
            _schoolClassInterface = schoolClassInterface;
            _mapper = mapper;

        }
        public async Task<Result<SchoolClassByIdResponse>> Handle(SchoolClassByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {

                var schoolClassById = await _schoolClassInterface.GetSchoolClassById(request.id);
                return Result<SchoolClassByIdResponse>.Success(schoolClassById.Data);

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Class by using id", ex);
            }
        }
    }
    
}
