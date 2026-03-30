using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.Repository
{
    public class SpRepository : ISpRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<SpRepository> _logger;

        public SpRepository(
            IConfiguration configuration,
            ILogger<SpRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;


        }

        #region Execute (Insert/Update/Delete)
        public async Task<int> ExecuteAsync(string storedProcedure, DynamicParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                _logger.LogInformation("Executing SP (NonQuery): {StoredProcedure}", storedProcedure);

                return await connection.ExecuteAsync(
                    new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        cancellationToken: cancellationToken
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP (NonQuery): {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        #endregion

        public async Task<(IEnumerable<T1>, IEnumerable<T2>)> ExecuteMultipleAsync<T1, T2>(string storedProcedure, DynamicParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                _logger.LogInformation("Executing SP (Multiple): {StoredProcedure}", storedProcedure);

                using var multi = await connection.QueryMultipleAsync(
                    new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        cancellationToken: cancellationToken
                    )
                );

                var result1 = await multi.ReadAsync<T1>();
                var result2 = await multi.ReadAsync<T2>();

                return (result1, result2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP (Multiple): {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<T?> ExecuteSingleAsync<T>(string storedProcedure, DynamicParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                _logger.LogInformation("Executing SP (Single): {StoredProcedure}", storedProcedure);

                return await connection.QueryFirstOrDefaultAsync<T>(
                    new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        cancellationToken: cancellationToken
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP (Single): {StoredProcedure}", storedProcedure);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string storedProcedure, DynamicParameters parameters, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);

                _logger.LogInformation("Executing SP: {StoredProcedure}", storedProcedure);

                return await connection.QueryAsync<TEntity>(
                    new CommandDefinition(
                        storedProcedure,
                        parameters,
                        commandType: CommandType.StoredProcedure,
                        cancellationToken: cancellationToken
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing SP: {StoredProcedure}", storedProcedure);
                throw;
            }
        }

    }
}
