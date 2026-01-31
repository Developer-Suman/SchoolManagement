using AutoMapper;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.StaffAttendanceRegister;
using ES.Staff.Application.Staff.Command.TeacherAttendanceQR;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UpdateAcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeam;
using ES.Staff.Application.Staff.Queries.AcademicTeamById;
using ES.Staff.Application.Staff.Queries.FilterAcademicTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;
using TN.Shared.Domain.Entities.Finance;
using TN.Shared.Domain.Entities.Staff;
using TN.Shared.Domain.Entities.Students;
using TN.Shared.Domain.ExtensionMethod.Pagination;

namespace ES.Staff.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region TeacherAttendance
            CreateMap<TeacherAttendanceQRResponse, TeacherAttendanceQRCommand>().ReverseMap();
            CreateMap<StaffAttendanceregister, StaffAttendanceregisterResponse>().ReverseMap();
            CreateMap<StaffAttendanceregisterCommand, StaffAttendanceregisterResponse>().ReverseMap();
            #endregion
            #region AssignedAndUnAssign Class
            CreateMap<UnAssignClassResponse, UnAssignClassCommand>().ReverseMap();
            CreateMap<AssignClassResponse, AssignClassCommand>().ReverseMap();
            #endregion
            #region AcademicTeam

            CreateMap<AcademicTeam, UpdateAcademicTeamCommand>().ReverseMap();
            CreateMap<AcademicTeamByIdResponse, AcademicTeam>().ReverseMap();
            CreateMap<AcademicTeamResponse, AcademicTeam>().ReverseMap();
            CreateMap<PagedResult<AcademicTeam>, PagedResult<AcademicTeamResponse>>().ReverseMap();

            CreateMap<AddAcademicTeamResponse, AddAcademicTeamCommand>().ReverseMap();
            CreateMap<AddAcademicTeamResponse, AcademicTeam>().ReverseMap();

            CreateMap<FilterAcademicTeamResponse, AcademicTeam>().ReverseMap();
            CreateMap<PagedResult<AcademicTeam>, PagedResult<FilterAcademicTeamResponse>>().ReverseMap();

            CreateMap<AddAcademicTeamResponse, AddAcademicTeamCommand>().ReverseMap();
            CreateMap<ApplicationUser, AddAcademicTeamCommand>().ReverseMap();

            #endregion
        }
    }
}
