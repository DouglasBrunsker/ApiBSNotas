using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brunsker.Bsnotas.Domain.Models;
using Brunsker.Bsnotasapi.Domain.Models;
using Brunsker.Bsnotas.WebApi.Helpers;
using System.Collections.Generic;
using System.IO;
using Brunsker.Bsnotasapi.Domain.Dtos;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Brunsker.Bsnotasapi.Domain.Interfaces;

namespace Brunsker.Bsnotas.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotaFiscalEntradaController : ControllerBase
    {
        private readonly INFEntradaRepository _rep;
        private readonly INFService _sevices;
        private readonly IWebHostEnvironment _env;
        private readonly ISefazApiAdapter _sefazServices;
        private readonly ILogger<NotaFiscalEntradaController> _logger;
        private readonly IMapper _mapper;

        public NotaFiscalEntradaController(INFEntradaRepository rep, INFService sevices, IWebHostEnvironment env,
                                           ISefazApiAdapter sefazServices, ILogger<NotaFiscalEntradaController> logger, IMapper mapper)
        {
            _rep = rep;
            _sevices = sevices;
            _env = env;
            _sefazServices = sefazServices;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet("BuscarEmpresas/{seqCliente}")]
        public async Task<IActionResult> BuscarEmpresasAsync(long seqCliente)
        {

            var listaEmpresas = await _rep.BuscarEmpresasAsync(seqCliente);

            if (listaEmpresas != null)
            {
                return Ok(listaEmpresas);
            }

            return NoContent();
        }

        [HttpPost("BuscarTotalizadoresGrafico")]
        public async Task<IActionResult> BuscarTotalizadoresGrafico(FiltroTotalizadores filtro)
        {
            var result = await _rep.BuscarTotalizadoresGraficoAsync(filtro);

            return Ok(result);
        }

        [HttpPost("BuscarTotalizadores")]
        public async Task<IActionResult> BuscarTotalizadoresAsync(FiltroTotalizadores filtro)
        {
            var totalizadores = await _rep.BuscarTotalizadoresAsync(filtro.DataInicial, filtro.DataFinal, filtro.SeqCliente);

            if (totalizadores != null)
            {
                return Ok(totalizadores);
            }

            return NoContent();
        }

        [HttpPost("BuscarNotasFiscaisEntrada/{index}/{length}")]
        public async Task<IActionResult> BuscarNotasFiscaisEntradaAsync(ParametrosPesquisaNfEntrada pesquisa, int index, int length)
        {
            var nfEntrada = await _rep.BuscarNotasFiscaisEntradaAsync(pesquisa);

            var result = new Pagination<NF>(index, length, nfEntrada);

            if (nfEntrada != null)
            {
                return Ok(result);
            }

            return NoContent();
        }

        [HttpGet("BuscarCfopNotaFiscalEntrada/{seqcliente}")]
        public async Task<IActionResult> BuscarCfopNotaFiscalEntradaAsync(long seqcliente)
        {
            var cfop = await _rep.BuscarCfopNotaFiscalEntradaAsync(seqcliente);

            if (cfop != null)
            {
                return Ok(cfop);
            }

            return NoContent();
        }

        [HttpGet("BuscarFornecedores/{seqcliente}")]
        public async Task<IActionResult> BuscarFornecedoresAsync(long seqcliente)
        {
            var fornecedores = await _rep.BuscarFornecedoresAsync(seqcliente);

            if (fornecedores != null)
            {
                return Ok(fornecedores);
            }

            return NoContent();
        }

        [HttpGet("GerarPdf/{chave}")]
        public async Task<IActionResult> GerarPdfAsync(string chave)
        {
            string xml = await _rep.SelectArquivoXml(chave);

            var pdf = await _sevices.GerarPdfAsync(xml);

            if (pdf != null)
            {
                var file = File(pdf, "application/pdf");

                return file;
            }

            return NoContent();
        }
        [HttpGet("GerarCCePDF/{chave}")]
        public async Task<IActionResult> GerarCCePDF(string chave)
        {
            string xml = await _rep.SelectArquivoXmlCCe(chave);

            var pdf = await _sevices.GerarCCeAsync(xml);

            if (pdf != null)
            {
                var file = File(pdf, "application/pdf");

                return file;
            }

            return NoContent();
        }

        [HttpPost("ExportaExcel")]
        public async Task<IActionResult> ExportaExcelAsync(ParametrosPesquisaNfEntrada filtro)
        {
            var notas = await _rep.BuscarNotasFiscaisEntradaAsync(filtro);

            var notasDto = _mapper.Map<IEnumerable<NFeToExport>>(notas);

            MemoryStream excelMemoryStream = _sevices.ExportaExcel(notasDto);

            if (excelMemoryStream == null)
            {
                return NoContent();
            }
            return File(excelMemoryStream, "application/vnd.ms-excel");
        }

        [HttpPost("ExportaXml")]
        public IActionResult ExportaXml(IEnumerable<NF> filtro)
        {
            var bytes = _sevices.ExportaXmls(filtro);

            if (bytes.Length > 0)
            {
                return File(bytes, "application/zip");
            }
            return NoContent();
        }

        [HttpPost("ExportaPdfs")]
        public async Task<IActionResult> ExportaPdf(IEnumerable<NF> filtro)
        {
            var bytes = await _sevices.ExportaPdfs(filtro);

            if (bytes.Length > 0)
            {
                return File(bytes, "application/zip", "Danfes.zip");
            }
            return NoContent();
        }

        [HttpPost("Manifestacao")]
        public async Task<IActionResult> Manifestacao(IEnumerable<Manifestacao> manifestacoes)
        {
            try
            {
                if (manifestacoes.Any())
                {
                    foreach (var manifestacao in manifestacoes)
                    {
                        await _sefazServices.ManifestaNotas(manifestacao, Path.Combine(_env.WebRootPath, "certificados/"));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
            }

            return Ok();
        }

        [HttpPost("ValidacaoPreEntrada")]
        public async Task<IActionResult> ValidacaoPreEntrada(ValidarPreEntrada validar)
        {
            var result = await _rep.ValidaPreEntrada(validar);

            return Ok(result);
        }

        [HttpPost("ProcessaPreEntrada")]
        public async Task<IActionResult> ProcessaPreEntrada(IEnumerable<ItensPedidoPre> itens)
        {
            if (itens.Any())
            {
                foreach (var item in itens)
                {
                    await _rep.ProcessaPreEntrada(item);
                }
            }

            return NoContent();
        }

        [HttpGet("BuscaPedidosAssociados/{seqCliente}/{chave}")]
        public async Task<IEnumerable<PedidoAssociado>> BuscaPedidosAssociados(string chave, int seqCliente)
        {
            var pedidos = await _rep.SelectPedidosAssociados(chave, seqCliente);

            return pedidos;
        }

        [HttpPost("BuscaItensPedido")]
        public async Task<IEnumerable<ItemPedido>> BuscaItensPedido(PesquisaItensPedido pesq)
        {
            var itens = await _rep.SelectItensPedido(pesq);

            return itens;
        }

        [HttpPost("BuscarContas/{seqCliente}")]
        public async Task<IActionResult> BuscarContas(long seqCliente)
        {
            var contas = await _rep.SelectContas(seqCliente);

            return Ok(contas);
        }
    }
}