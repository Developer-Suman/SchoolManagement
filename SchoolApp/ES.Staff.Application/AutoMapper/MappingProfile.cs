using AutoMapper;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using ES.Staff.Application.Staff.Command.AssignClassToAcademicTeam;
using ES.Staff.Application.Staff.Command.UnAssignedClassToAcademicTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Authentication.Domain.Entities;

namespace ES.Staff.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            #region AssignedAndUnAssign Class
            CreateMap<UnAssignClassResponse, UnAssignClassCommand>().ReverseMap();
            CreateMap<AssignClassResponse, AssignClassCommand>().ReverseMap();
            #endregion
            #region AcademicTeam

            CreateMap<AddAcademicTeamResponse, AddAcademicTeamCommand>().ReverseMap();
            CreateMap<ApplicationUser, AddAcademicTeamCommand>().ReverseMap();

            #endregion
        }
    }
}
