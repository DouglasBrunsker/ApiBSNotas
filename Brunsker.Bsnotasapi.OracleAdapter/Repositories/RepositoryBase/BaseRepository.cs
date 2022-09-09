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

namespace Brunsker.Bsnotas.OracleAdapter.Repositories.RepositoryBase
{
    public abstract class BaseRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<object> _logger;

        public BaseRepository(IConfiguration configuration, ILogger<object> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn, TEntry>(TEntry entry, string sql)
            where TReturn : class
            where TEntry : class
        {
            try
            {
                using (var oracleConnection = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    if (oracleConnection.State == ConnectionState.Closed) 
                        oracleConnection.Open();

                    var oracleDynamicParameters = new OracleDynamicParameters();
                    var properties = entry.GetType().GetProperties();
                    
                    foreach (var propertyInfo in properties)
                    {
                        var nameParamProcedure = (AttributeNameProcedure.NameParamProcedure)propertyInfo.GetCustomAttributes(typeof(AttributeNameProcedure.NameParamProcedure), inherit: false).FirstOrDefault();
                        
                        if (nameParamProcedure != null)
                            oracleDynamicParameters.Add(nameParamProcedure.Name ?? ("p" + propertyInfo.Name), propertyInfo.GetValue(entry));
                    }

                    oracleDynamicParameters.Add("CUR_OUT", null, OracleMappingType.RefCursor, ParameterDirection.Output);

                    return await oracleConnection.QueryAsync<TReturn>(sql, oracleDynamicParameters, null, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }

            return null;
        }
    }
}
