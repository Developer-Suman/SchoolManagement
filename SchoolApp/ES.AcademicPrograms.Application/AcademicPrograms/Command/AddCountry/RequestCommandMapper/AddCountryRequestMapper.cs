using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.AcademicPrograms.Application.AcademicPrograms.Command.AddCountry.RequestCommandMapper
{
    public static class AddCountryRequestMapper
    {
        public static AddCountryCommand ToCommand(this AddCountryRequest request)
        {
            return new AddCountryCommand
                (
                request.name
                );
            
        }
    }
}
