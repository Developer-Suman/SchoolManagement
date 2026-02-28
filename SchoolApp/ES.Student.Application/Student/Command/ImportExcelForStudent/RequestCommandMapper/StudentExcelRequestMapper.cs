using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Student.Application.Student.Command.ImportExcelForStudent.RequestCommandMapper
{
    public static class StudentExcelRequestMapper
    {
        public static StudentExcelCommand ToCommand(this StudentExcelRequest request)
        {
            return new StudentExcelCommand
                (
                request.formFile
                );
        }
    }
}
