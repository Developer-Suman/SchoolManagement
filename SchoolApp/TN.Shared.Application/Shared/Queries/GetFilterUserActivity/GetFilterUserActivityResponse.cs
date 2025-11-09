using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Application.Shared.Queries.GetFilterUserActivity
{
    public record GetFilterUserActivityResponse
   (
        string? userId,
            string? tableName,
            string? primaryKey,
            string? fieldName,
            string? typeOfChange,
            string? oldValue,
            string? newValue,
            string? hashValue
        );
}
