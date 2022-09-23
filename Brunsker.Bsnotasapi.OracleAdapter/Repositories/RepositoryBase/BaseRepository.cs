using Oracle.ManagedDataAccess.Client;
using System.Data;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper.Oracle;
using System.Collections.Generic;
using Dapper;
using Microsoft.Extensions.Logging;
using Brunsker.AutoMapperProcedure.CustomAttributes;
using System.Linq;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace Brunsker.Bsnotas.OracleAdapter.Repositories.RepositoryBase
{
    public abstract class BaseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<object> _logger;
        private string _connectionString;

        public BaseRepository(IConfiguration configuration, ILogger<object> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = _configuration.GetConnectionString("OracleConnection");
        }

        protected async Task<IEnumerable<TReturn>> QueryAsync<TReturn, TEntry>(TEntry entry, string sql)
            where TReturn : class
            where TEntry : class
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    OpenOracleConnection(oracleConnection);

                    var oracleDynamicParameters = BuildDefaultOracleDynamicParameters(entry);

                    return await oracleConnection.QueryAsync<TReturn>(sql, oracleDynamicParameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError("Error: " + exception.Message);

                throw;
            }
        }

        protected async Task<TReturn> ParameterLessQueryFirstOrDefaultAsyncReturnObject<TReturn>(string sql)
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    OpenOracleConnection(oracleConnection);

                    var oracleDynamicParameters = new OracleDynamicParameters();
                    
                    oracleDynamicParameters.Add("CUR_OUT", null, OracleMappingType.RefCursor, ParameterDirection.Output);

                    return await oracleConnection.QueryFirstOrDefaultAsync<TReturn>(sql, oracleDynamicParameters);
                }
            }
            catch(Exception exception)
            {
                _logger.LogError("Error: " + exception.Message);

                throw;
            }
        }

        protected async Task<TReturn> QueryFirstOrDefaultAsync<TReturn, TEntry>(TEntry entry, string sql)
            where TReturn : class
            where TEntry : class
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    OpenOracleConnection(oracleConnection);

                    var oracleDynamicParameters = BuildDefaultOracleDynamicParameters(entry);

                    return await oracleConnection.QueryFirstOrDefaultAsync<TReturn>(sql, oracleDynamicParameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch(Exception exception)
            {
                _logger.LogError("Error: " + exception.Message);

                throw;
            }
        }

        protected async Task<TReturn> QueryFirstOrDefaultAsyncWithOracleDynamicParameters<TReturn>(OracleDynamicParameters oracleDynamicParameters, string sql)
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_connectionString))
                {
                    OpenOracleConnection(oracleConnection);

                    return await oracleConnection.QueryFirstOrDefaultAsync<TReturn>(sql, oracleDynamicParameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch(Exception exception)
            {
                _logger.LogError("Error: " + exception.Message);

                throw;
            }
        }

        private void OpenOracleConnection(OracleConnection oracleConnection)
        {
            if (oracleConnection.State == ConnectionState.Closed)
                oracleConnection.Open();
        }

        private OracleDynamicParameters BuildDefaultOracleDynamicParameters<TEntry>(TEntry entry)
            where TEntry : class
        {
            var oracleDynamicParameters = new OracleDynamicParameters();
            var properties = entry.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                var nameParamProcedure = (AttributeNameProcedure.NameParamProcedure)propertyInfo.GetCustomAttributes(typeof(AttributeNameProcedure.NameParamProcedure), inherit: false).FirstOrDefault();

                if (nameParamProcedure != null)
                    oracleDynamicParameters.Add(nameParamProcedure.Name ?? ("p" + propertyInfo.Name), propertyInfo.GetValue(entry));
            }

            oracleDynamicParameters.Add("CUR_OUT", null, OracleMappingType.RefCursor, ParameterDirection.Output);

            return oracleDynamicParameters;
        }
    }
}
