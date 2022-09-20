﻿using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.OracleAdapter.Repositories.RepositoryBase
{
    public sealed class NfseServicoRepository : BaseRepository, INfseServiceRepository
    {
        public NfseServicoRepository(IConfiguration configuration, ILogger<object> logger) : base(configuration, logger)
        {
        }

        public async Task<IEnumerable<Totalizador>> GetTotalizadoresAsync(SearchNfse searchNfse) =>
            await QueryAsync<Totalizador, SearchNfse>(searchNfse, "PKG_BS_NF_SERVICO2.PESQ_TOTALIZADORES");

        public async Task<IEnumerable<NotasDia>> GetRecebidasDiaAsync(SearchNfse searchNfse) =>
            await QueryAsync<NotasDia, SearchNfse>(searchNfse, "PKG_BS_NF_SERVICO2.PESQ_NFSE_RECEBIDAS_DIA");

        public async Task<IEnumerable<Nfse>> GetNfseAsync(SearchNf searchNf) =>
            await QueryAsync<Nfse, SearchNf>(searchNf, "PKG_BS_NF_SERVICO2.PESQ_NFSE");

        public async Task<IEnumerable<Company>> GetEmpresasAsync(SearchCompany searchCompany) =>
            await QueryAsync<Company, SearchCompany>(searchCompany, "PKG_BS_NF_SERVICO2.PESQ_EMPRESAS");

        public async Task<Pdf> GeneratePdfAsync(GeneratePdf generatePdf) =>
            await QueryFirstOrDefaultAsync<Pdf, GeneratePdf>(generatePdf, "PKG_BS_NF_SERVICO2.EMITEPDF_NOTA");
    }
}