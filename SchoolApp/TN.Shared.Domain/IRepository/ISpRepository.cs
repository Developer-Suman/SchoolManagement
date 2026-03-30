using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TN.Shared.Domain.IRepository
{
    public interface ISpRepository
    {
        Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(
            string storedProcedure,
            DynamicParameters dynamicParameters,
            CancellationToken cancellationToken = default);
        Task<T?> ExecuteSingleAsync<T>(
            string storedProcedure,
            DynamicParameters parameters,
            CancellationToken cancellationToken = default);

        Task<int> ExecuteAsync(
            string storedProcedure,
            DynamicParameters parameters,
            CancellationToken cancellationToken = default);

        Task<(IEnumerable<T1>, IEnumerable<T2>)> ExecuteMultipleAsync<T1, T2>(
            string storedProcedure,
            DynamicParameters parameters,
            CancellationToken cancellationToken = default);

    }
}
