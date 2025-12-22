using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Feetype
            CreateMap<FeeType, AddFeeTypeResponse>().ReverseMap();
            CreateMap<FilterFeeTypeResponse, FeeType>().ReverseMap();
            CreateMap<PagedResult<FeeType>, PagedResult<FilterFeeTypeResponse>>().ReverseMap();
            #endregion

            #region FeeStructure
            CreateMap<FeeStructure, AddFeeStructureResponse>().ReverseMap();
            CreateMap<FilterFeeStructureResponse, FeeStructure>().ReverseMap();
            CreateMap<PagedResult<FeeStructure>, PagedResult<FilterFeeStructureResponse>>().ReverseMap();
            #endregion

            #region StudentFee
            CreateMap<StudentFee, AddStudentFeeResponse>().ReverseMap();
            CreateMap<FilterStudentFeeResponse, StudentFee>().ReverseMap();
            CreateMap<PagedResult<StudentFee>, PagedResult<FilterStudentFeeResponse>>().ReverseMap();
            #endregion

        }
    }
}
