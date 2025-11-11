using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.DeleteCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.DeleteIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificateById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.Entities.Academics;
using TN.Shared.Domain.Entities.Certificates;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Certificate.Application.AutoMapper
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            #region IssuedCertificate

            CreateMap<IssuedCertificate, DeleteIssuedCertificateCommand>().ReverseMap();

            CreateMap<IssuedCertificateByIdQuery, IssuedCertificate>().ReverseMap();

            CreateMap<IssuedCertificateByIdResponse, IssuedCertificate>().ReverseMap();

            CreateMap<IssuedCertificateResponse, IssuedCertificate>().ReverseMap();
            CreateMap<AddIssuedCertificateCommand, IssuedCertificate>().ReverseMap();
            CreateMap<AddIssuedCertificateResponse, IssuedCertificate>().ReverseMap();
            CreateMap<AddIssuedCertificateCommand, AddIssuedCertificateResponse>().ReverseMap();

            CreateMap<PagedResult<IssuedCertificate>, PagedResult<IssuedCertificateResponse>>().ReverseMap();

            CreateMap<FilterIssuedCertificateResponse, IssuedCertificate>().ReverseMap();
            CreateMap<PagedResult<IssuedCertificate>, PagedResult<FilterIssuedCertificateResponse>>().ReverseMap();
            #endregion

            #region CertificateTemplate

            CreateMap<CertificateTemplate, DeleteCertificateTemplateCommand>().ReverseMap();

            CreateMap<CertificateTemplateByIdQuery, CertificateTemplate>().ReverseMap();

            CreateMap<CertificateTemplateByIdResponse, CertificateTemplate>().ReverseMap();

            CreateMap<CertificateTemplateResponse, CertificateTemplate>().ReverseMap();
            CreateMap<AddCertificateTemplateCommand, CertificateTemplate>().ReverseMap();
            CreateMap<AddCertificateTemplateResponse, CertificateTemplate>().ReverseMap();
            CreateMap<AddCertificateTemplateCommand, AddCertificateTemplateResponse>().ReverseMap();

            CreateMap<PagedResult<CertificateTemplate>, PagedResult<CertificateTemplateResponse>>().ReverseMap();

            CreateMap<FilterCertificateTemplateResponse, CertificateTemplate>().ReverseMap();
            CreateMap<PagedResult<CertificateTemplate>, PagedResult<FilterCertificateTemplateResponse>>().ReverseMap();
            #endregion
        }

    }
}
