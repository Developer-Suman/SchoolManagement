using AutoMapper;
using ES.Staff.Application.Staff.Command.AddAcademicTeam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Staff.Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region AcademicTeam

            CreateMap<AddAcademicTeamResponse, AddAcademicTeamCommand>().ReverseMap();

            #endregion
        }
    }
}
