using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddUniversity.RequestCommandMapper
{
    public static class AddUniversityRequestMapper
    {
        public static AddUniversityCommand ToCommand(this AddUniversityRequest request)
        {
            return new AddUniversityCommand
                (
                    request.name,
                    request.country,
                    request.descriptions,
                    request.website,
                    request.globalRanking

                );
            
        }
    }
}
