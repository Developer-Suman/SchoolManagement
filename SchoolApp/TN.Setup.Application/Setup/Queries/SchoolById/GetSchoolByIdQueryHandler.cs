
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.SchoolById
{
    public sealed class GetSchoolByIdQueryHandler : IRequestHandler<GetSchoolByIdQuery, Result<GetSchoolByIdResponse>>
    {
        private readonly ISchoolServices  _schoolServices;
        private readonly IMapper _mapper;

        public GetSchoolByIdQueryHandler(ISchoolServices schoolServices,IMapper mapper)
        {
            _schoolServices = schoolServices;
            _mapper=mapper;
            
        }

        public async Task<Result<GetSchoolByIdResponse>> Handle(GetSchoolByIdQuery request, CancellationToken cancellationToken)
        {
            try 
            {

                var schoolById = await _schoolServices.GetSchoolById(request.id);

                return Result<GetSchoolByIdResponse>.Success(schoolById.Data);

            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while fetching School by Id",ex);
            
            }
        }
    }
}
