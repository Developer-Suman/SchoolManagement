
using AutoMapper;
using MediatR;
using TN.Setup.Application.ServiceInterface;
using TN.Shared.Domain.Abstractions;

namespace TN.Setup.Application.Setup.Queries.FilterSchoolByDate
{
    public class FilterSchoolByDateQueryHandler : IRequestHandler<FilterSchoolByDateQuery, Result<IEnumerable<FilterSchoolByDateQueryResponse>>>
    {
        private readonly ISchoolServices _schoolServices;
        private readonly IMapper _mapper;

        public FilterSchoolByDateQueryHandler(ISchoolServices schoolServices, IMapper mapper)
        {

            _schoolServices = schoolServices;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<FilterSchoolByDateQueryResponse>>> Handle(FilterSchoolByDateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var filterSchool = await _schoolServices.GetSchoolFilter(request.FilterSchoolDTOs, cancellationToken);

                if (!filterSchool.IsSuccess || filterSchool.Data == null)
                {
                    return Result<IEnumerable<FilterSchoolByDateQueryResponse>>.Failure(filterSchool.Message);
                }

                var filterCompanyResult = _mapper.Map<List<FilterSchoolByDateQueryResponse>>(filterSchool.Data);

                return Result<IEnumerable<FilterSchoolByDateQueryResponse>>.Success(filterCompanyResult);
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<FilterSchoolByDateQueryResponse>>.Failure(
                    $"An error occurred while fetching Company  by date: {ex.Message}");
            }
        }
    }
}
