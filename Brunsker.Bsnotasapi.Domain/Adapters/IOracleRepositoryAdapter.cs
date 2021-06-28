﻿using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brunsker.Bsnotas.Domain.Adapters
{
    public interface IOracleRepositoryAdapter
    {
        Task<string> SelectArquivoXml(string chave);
        Task<string> SelectArquivoXmlCCe(string chave);
        Task<IEnumerable<EmpresasCliente>> BuscarEmpresasAsync(long seqCliente);
        Task<Totalizadores> BuscarTotalizadoresAsync(DateTime dataInicio, DateTime dataFim, int seqCliente);
        Task<IEnumerable<TotalizadorNotasPorDia>> BuscarTotalizadoresGraficoAsync(FiltroTotalizadores filtro);
        Task<IEnumerable<NotaFiscalEntrada>> BuscarNotasFiscaisEntradaAsync(ParametrosPesquisaNfEntrada pesquisa);
        Task<IEnumerable<CFOP>> BuscarCfopNotaFiscalEntradaAsync(long seqCliente);
        Task<IEnumerable<DetalheNotaFiscalEntrada>> BuscarDetalhesNotaFiscalEntradaAsync(long seqCliente, string chaveNfe);
        Task<IEnumerable<Fornecedores>> BuscarFornecedoresAsync(long seqCliente);
        Task<RecepcaoEvento> SelectRelacaoWebServices(int seq_cliente, string cnpj);
        Task ConfirmaManifestacao(string chave, string codigoEvento);
        Task InsertUsuario(Usuario usuario);
        Task<Usuario> SelectUsuarioPorEmail(string email);
        Task<Usuario> Login(string login, string senha);
        Task<IEnumerable<ResultadoValidacaoPreEntrada>> ValidaPreEntrada(ValidarPreEntrada validar);
        Task ProcessaPreEntrada(string chave, int seqCliente, long? numped);
        Task<IEnumerable<PedidoAssociado>> SelectPedidosAssociados(string chave, int seqCliente);
        Task<ParametrosCliente> SelectParametros(int seqCliente);
        Task<IEnumerable<ItemPedido>> SelectItensPedido(PesquisaItensPedido pesq);
    }
}
