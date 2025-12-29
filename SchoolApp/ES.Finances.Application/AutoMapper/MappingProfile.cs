using AutoMapper;
using ES.Finances.Application.Finance.Command.Fee.AddFeeStructure;
using ES.Finances.Application.Finance.Command.Fee.AddFeeType;
using ES.Finances.Application.Finance.Command.Fee.AddStudentFee;
using ES.Finances.Application.Finance.Command.Fee.UpdateFeeStructure;

using ES.Finances.Application.Finance.Command.Fee.UpdateFeeType;
using ES.Finances.Application.Finance.Command.PaymentRecords.AddpaymentsRecords;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FeeStructureById;
using ES.Finances.Application.Finance.Queries.Fee.Feetype;
using ES.Finances.Application.Finance.Queries.Fee.FeetypeById;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeeStructure;
using ES.Finances.Application.Finance.Queries.Fee.FilterFeetype;
using ES.Finances.Application.Finance.Queries.Fee.FilterStudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFee;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeById;
using ES.Finances.Application.Finance.Queries.Fee.StudentFeeSummary;
using ES.Finances.Application.Finance.Queries.PaymentsRecords.PaymentsRecordsById;
using TN.Account.Application.Account.Command.AddLedger;
using TN.Account.Domain.Entities;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Finances.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region StudentFeeSummary
 
            #endregion

            #region AssignMonthlyFee
            CreateMap<StudentFeeSummaryResponse, StudentFee>().ReverseMap();
            CreateMap<Ledger,AddLedgerResponse>();
            #endregion

            #region PaymentRecords

            CreateMap<AddpaymentsRecordsResponse, PaymentsRecords>().ReverseMap();
            CreateMap<PaymentsRecordsByIdResponse, PaymentsRecords>().ReverseMap();
            #endregion
            #region Feetype
            CreateMap<FeeType, FeetypeByidResponse>().ReverseMap();
            CreateMap<FeeType, UpdateFeeTypeCommand>().ReverseMap();
            CreateMap<FeeType, AddFeeTypeResponse>().ReverseMap();
            CreateMap<FilterFeeTypeResponse, FeeType>().ReverseMap();
            CreateMap<PagedResult<FeeType>, PagedResult<FilterFeeTypeResponse>>().ReverseMap();
            CreateMap<FeeTypeResponse, FeeType>().ReverseMap();
            CreateMap<PagedResult<FeeType>, PagedResult<FeeTypeResponse>>().ReverseMap();
            #endregion

            #region FeeStructure
            CreateMap<FeeStructure, UpdateFeeStructureCommand>().ReverseMap();

            CreateMap<FeeStructure, FeeStructureByIdResponse>().ReverseMap();
            CreateMap<FeeStructure, AddFeeStructureResponse>().ReverseMap();
            CreateMap<FilterFeeStructureResponse, FeeStructure>().ReverseMap();
            CreateMap<PagedResult<FeeStructure>, PagedResult<FilterFeeStructureResponse>>().ReverseMap();
            CreateMap<FeeStructureResponse, FeeStructure>().ReverseMap();
            CreateMap<PagedResult<FeeStructure>, PagedResult<FeeStructureResponse>>().ReverseMap();
            #endregion

            #region StudentFee
            CreateMap<StudentFee, AddStudentFeeResponse>().ReverseMap();
            CreateMap<StudentFee, StudentFeeByIdResponse>().ReverseMap();
            CreateMap<FilterStudentFeeResponse, StudentFee>().ReverseMap();
            CreateMap<PagedResult<StudentFee>, PagedResult<FilterStudentFeeResponse>>().ReverseMap();
            CreateMap<StudentFeeResponse, StudentFee>().ReverseMap();
            CreateMap<PagedResult<StudentFee>, PagedResult<StudentFeeResponse>>().ReverseMap();
            #endregion

        }
    }
}
