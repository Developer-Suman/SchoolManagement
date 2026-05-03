using AutoMapper;
using ES.Crm.Finance.Application.CrmFinance.Command.InstallmentsPlan.AddInstallmentsPlan;
using ES.Crm.Finance.Application.CrmFinance.Command.Payments.Addpayments;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.FilterInstallmentPlan;
using ES.Crm.Finance.Application.CrmFinance.Queries.InstallmentsPlan.InstallmentPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Crm.Finance;
using TN.Shared.Domain.Entities.Crm.Visa;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Crm.Finance.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Installment Plan
            CreateMap<AddInstallmentsPlanCommand, InstallmentPlan>().ReverseMap();
            CreateMap<AddInstallmentsPlanResponse, InstallmentPlan>().ReverseMap();
            CreateMap<AddInstallmentsPlanCommand, AddInstallmentsPlanResponse>().ReverseMap();

            CreateMap<FilterInstallmentPlanResponse, InstallmentPlan>().ReverseMap();
            CreateMap<PagedResult<InstallmentPlan>, PagedResult<FilterInstallmentPlanResponse>>().ReverseMap();

            CreateMap<InstallmentPlanResponse, InstallmentPlan>().ReverseMap();
            #endregion

            #region Payment
            CreateMap<AddPaymentsCommand, CrmPayment>().ReverseMap();
            CreateMap<AddPaymentsResponse, CrmPayment>().ReverseMap();
            CreateMap<AddPaymentsCommand, AddPaymentsResponse>().ReverseMap();
            #endregion
        }
    }
}
