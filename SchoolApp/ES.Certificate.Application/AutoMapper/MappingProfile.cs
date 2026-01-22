using AutoMapper;
using ES.Certificate.Application.Certificates.Command.AddCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.AddIssuedCertificate;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.DeleteAwards;
using ES.Certificate.Application.Certificates.Command.Awards.SchoolAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.AddAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.DeleteAwards;
using ES.Certificate.Application.Certificates.Command.Awards.StudentsAwards.UpdateAwards;
using ES.Certificate.Application.Certificates.Command.DeleteCertificateTemplate;
using ES.Certificate.Application.Certificates.Command.DeleteIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.CertificateTemplateById;
using ES.Certificate.Application.Certificates.Queries.FilterCertificateTemplate;
using ES.Certificate.Application.Certificates.Queries.FilterIssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.GenerateCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificate;
using ES.Certificate.Application.Certificates.Queries.IssuedCertificateById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.AwardsById;
using ES.Certificate.Application.Certificates.Queries.SchoolAwards.FilterSchoolAwards;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.Awards;
using ES.Certificate.Application.Certificates.Queries.StudentsAwards.AwardsById;
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

            #region StudentsAwards
            CreateMap<StudentsAward, DeleteAwardsCommand>().ReverseMap();

            CreateMap<AwardsByIdQuery, StudentsAward>().ReverseMap();

            CreateMap<AwardsByIdResponse, StudentsAward>().ReverseMap();

            CreateMap<UpdateAwardsCommand, StudentsAward>().ReverseMap();
            CreateMap<AwardsResponse, StudentsAward>().ReverseMap();
            CreateMap<AddAwardsCommand, StudentsAward>().ReverseMap();
            CreateMap<AddAwardsResponse, StudentsAward>().ReverseMap();
            CreateMap<AddAwardsCommand, AddAwardsResponse>().ReverseMap();

            CreateMap<PagedResult<StudentsAward>, PagedResult<AwardsResponse>>().ReverseMap();

            CreateMap<FilterSchoolAwardsResponse, SchoolAwards>().ReverseMap();
            CreateMap<PagedResult<SchoolAwards>, PagedResult<FilterSchoolAwardsResponse>>().ReverseMap();
            #endregion



            #region SchoolAwards
            CreateMap<SchoolAwards, DeleteSchoolAwardsCommand>().ReverseMap();

            CreateMap<SchoolAwardsByIdQuery, SchoolAwards>().ReverseMap();

            CreateMap<SchoolAwardsByIdResponse, SchoolAwards>().ReverseMap();

            CreateMap<UpdateSchoolAwardsCommand, SchoolAwards>().ReverseMap();
            CreateMap<SchoolAwardsResponse, SchoolAwards>().ReverseMap();
            CreateMap<AddSchoolAwardsCommand, SchoolAwards>().ReverseMap();
            CreateMap<AddSchoolAwardsResponse, SchoolAwards>().ReverseMap();
            CreateMap<AddSchoolAwardsCommand, AddSchoolAwardsResponse>().ReverseMap();

            CreateMap<PagedResult<SchoolAwards>, PagedResult<SchoolAwardsResponse>>().ReverseMap();

            //CreateMap<FilterIssuedCertificateResponse, SchoolAwards>().ReverseMap();
            //CreateMap<PagedResult<SchoolAwards>, PagedResult<FilterIssuedCertificateResponse>>().ReverseMap();
            #endregion


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

            #region Generate Certificate
            //CreateMap<Generat, GenerateCertificateResponse>().ReverseMap();
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
