using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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

        public SpRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("connectionString");

        }
        public async Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string storedProcedure, DynamicParameters parameters)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TEntity>(
                  storedProcedure,
                  parameters,
                  commandType: CommandType.StoredProcedure
              );
        }
    }
}
