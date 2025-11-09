using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Account.Domain.Entities;
using TN.Authentication.Domain.Entities;
using TN.Purchase.Domain.Entities;
using TN.Shared.Application.Shared.Command.CloseFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateCurrentFiscalYear;
using TN.Shared.Application.Shared.Command.UpdateExpiredDateItemStatusBySchool;
using TN.Shared.Application.Shared.Command.UpdateInventoryMethodBySchool;
using TN.Shared.Application.Shared.Command.UpdateItemStatusBySchool;
using TN.Shared.Application.Shared.Command.UpdateJournalRefBySchool;
using TN.Shared.Application.Shared.Command.UpdatePurchaseQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdatePurchaseRefNumberBySchool;
using TN.Shared.Application.Shared.Command.UpdatePurchaseReturnType;
using TN.Shared.Application.Shared.Command.UpdateSalesQuotationNumberType;
using TN.Shared.Application.Shared.Command.UpdateSalesReferenceNumberBySchool;
using TN.Shared.Application.Shared.Command.UpdateSalesReturnType;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInPurchase;
using TN.Shared.Application.Shared.Command.UpdateTaxStatusInSales;
using TN.Shared.Application.Shared.Queries.GetAllCurrentFiscalYear;
using TN.Shared.Application.Shared.Queries.GetFilterUserActivity;
using TN.Shared.Application.Shared.Queries.GetPurchaseQuotationNumber;
using TN.Shared.Application.Shared.Queries.GetPurchaseReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSalesQuotationNumberType;
using TN.Shared.Application.Shared.Queries.GetSalesReturnNumber;
using TN.Shared.Application.Shared.Queries.GetSelectedFiscalYear;
using TN.Shared.Domain.Entities.AuditLogs;
using TN.Shared.Domain.Entities.OrganizationSetUp;
using TN.Shared.Domain.ExtensionMethod.Pagination;


namespace TN.Shared.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region Inventory Settings
            CreateMap<CloseFiscalYearCommand, CloseFiscalYearResponse>().ReverseMap();
            CreateMap<UpdateFiscalYearResponse,UpdateFiscalYearCommand>().ReverseMap();
            CreateMap<GetAllFiscalYearQueryResponse, FiscalYears>().ReverseMap();
            CreateMap<PagedResult<FiscalYears>, PagedResult<GetAllFiscalYearQueryResponse>>().ReverseMap();
            CreateMap<UpdateItemStatusBySchoolResponse, UpdateItemStatusBySchoolCommand>().ReverseMap();
            //CreateMap<GetJournalRefByCompanyQueryResponse, CompanySettings>().ReverseMap();
            CreateMap<UpdateJournalRefBySchoolResponse, UpdateJournalRefBySchoolCommand>().ReverseMap();
            CreateMap<UpdateSalesReferenceNumberResponse, UpdateSalesReferenceNumberCommand>().ReverseMap();
           CreateMap<UpdateInventoryMethodResponse, UpdateInventoryMethodCommand>().ReverseMap();
            CreateMap<UpdatePurchaseReferenceNumberResponse, UpdatePurchaseReferenceNumberCommand>().ReverseMap();
           CreateMap<UpdateTaxStatusInPurchaseResponse, UpdateTaxStatusInPurchaseCommand>().ReverseMap();
            CreateMap<UpdateTaxStatusInSalesResponse, UpdateTaxStatusInSalesCommand>().ReverseMap();
            #endregion



            CreateMap<GetFilterUserActivityResponse, AuditLog>().ReverseMap();
            CreateMap<PagedResult<AuditLog>, PagedResult<GetFilterUserActivityResponse>>().ReverseMap();




            CreateMap<GetSelectedFiscalYearQueryResponse, FiscalYears>().ReverseMap();
            CreateMap<PagedResult<FiscalYears>, PagedResult<GetSelectedFiscalYearQueryResponse>>().ReverseMap();

            CreateMap<GetPurchaseReturnNumberQueryResponse,SchoolSettings>().ReverseMap();
            CreateMap<GetSalesReturnNumberQueryResponse, SchoolSettings>().ReverseMap();
          
            CreateMap<UpdatePurchaseReturnTypeResponse, UpdatePurchaseReturnTypeCommand>().ReverseMap();
           CreateMap<UpdateSalesReturnTypeResponse, UpdateSalesReturnTypeCommand>().ReverseMap();
            
            CreateMap<GetPurchaseQuotationNumberQueryResponse, SchoolSettings>().ReverseMap();
            CreateMap<GetSalesQuotationTypeQueryResponse, SchoolSettings>().ReverseMap();
       
            CreateMap<UpdatePurchaseQuotationTypeResponse, UpdatePurchaseQuotationTypeCommand>().ReverseMap();
            CreateMap<UpdateSalesQuotationTypeResponse, UpdateSalesQuotationTypeCommand>().ReverseMap();
        }
    }
}   
