using Brunsker.Bsnotas.Application.AutoMapperConfigurations;
using Brunsker.Bsnotas.Application.Interfaces;
using Brunsker.Bsnotas.Application.Requests.GeneratePdf;
using Brunsker.Bsnotas.Application.Requests.Searchs;
using Brunsker.Bsnotas.Application.Responses.Company;
using Brunsker.Bsnotas.Application.Responses.Notas;
using Brunsker.Bsnotas.Application.Responses.Pdf;
using Brunsker.Bsnotas.Application.Responses.Totalizador;
using Brunsker.Bsnotas.Domain.Interfaces;
using Brunsker.Bsnotas.Domain.Models;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using System.IO.Pipes;

namespace Brunsker.Bsnotas.Application.Services
{
    public sealed class NfseServicoService : INfseServicoService
    {
        private readonly INfseServiceRepository _nfseServiceRepository;
        private readonly ILogger<NfseServicoService> _logger;

        public NfseServicoService(INfseServiceRepository nfseServiceRepository, ILogger<NfseServicoService> logger)
        {
            _nfseServiceRepository = nfseServiceRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TotalizadorResponse>> GetTotalizadoresAsync(SearchNfseRequest searchNfseRequest)
        {
            var searchNfse = searchNfseRequest.MapTo<SearchNfseRequest, SearchNfse>();

            var totalizadoresEnumerable = await _nfseServiceRepository.GetTotalizadoresAsync(searchNfse);

            return totalizadoresEnumerable.MapTo<IEnumerable<Totalizador>, IEnumerable<TotalizadorResponse>>();
        }

        public async Task<IEnumerable<NotasDiaResponse>> GetRecebidasDiaAsync(SearchNfseRequest searchNfseRequest)
        {
            var searchNfse = searchNfseRequest.MapTo<SearchNfseRequest, SearchNfse>();

            var notasDiaEnumerable = await _nfseServiceRepository.GetRecebidasDiaAsync(searchNfse);

            return notasDiaEnumerable.MapTo<IEnumerable<NotasDia>, IEnumerable<NotasDiaResponse>>();
        }

        public async Task<IEnumerable<NfseResponse>> GetNfseAsync(SearchNfRequest searchNfRequest)
        {
            var searchNf = searchNfRequest.MapTo<SearchNfRequest, SearchNf>();

            var nfseEnumerable = await _nfseServiceRepository.GetNfseAsync(searchNf);

            return nfseEnumerable.MapTo<IEnumerable<Nfse>, IEnumerable<NfseResponse>>();
        }

        public async Task<IEnumerable<CompanyResponse>> GetEmpresasAsync(SearchCompanyRequest searchCompanyRequest)
        {
            var searchCompany = searchCompanyRequest.MapTo<SearchCompanyRequest, SearchCompany>();

            var companyEnumerable = await _nfseServiceRepository.GetEmpresasAsync(searchCompany);

            return companyEnumerable.MapTo<IEnumerable<Company>, IEnumerable<CompanyResponse>>();
        }

        public async Task<PdfResponse> GeneratePdfAsync(GeneratePdfRequest generatePdfRequest)
        {
            var generatePdf = generatePdfRequest.MapTo<GeneratePdfRequest, GeneratePdf>();

            var pdf = await _nfseServiceRepository.GeneratePdfAsync(generatePdf);

            return pdf.MapTo<Pdf, PdfResponse>();
        }

        public async Task<byte[]> ExportaNfseExcelAsync(IEnumerable<int> notasServicoIdEnumerable)
        {
            if (notasServicoIdEnumerable.Any())
            {
                var nfseList = await BuildNfseList(notasServicoIdEnumerable);

                return BuildExcelByteArray(nfseList);
            }

            return null;
        }

        public async Task<byte[]> ExportaNfseXmlsAsync(IEnumerable<int> notasServicoIdEnumerable)
        {
            try
            {
                var fileName = @"wwwroot\NFSE\xml_nfses" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".zip";

                if (notasServicoIdEnumerable.Any())
                {
                    var nfseList = await BuildNfseList(notasServicoIdEnumerable);

                    using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        CreateXmlToFile(fileStream, nfseList);
                    }

                    var fileBytes = File.ReadAllBytes(fileName);

                    File.Delete(fileName);

                    return fileBytes;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return null;
            }

            return null;
        }

        private void CreateXmlToFile(FileStream fileStream, List<Nfse> nfseList)
        {
            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Update))
            {
                foreach (var nfse in nfseList)
                {
                    var xmlDocument = new XmlDocument();

                    xmlDocument.LoadXml(nfse.ARQUIVO_XML);

                    var nfseArquivoXmlBytes = Encoding.ASCII.GetBytes(nfse.ARQUIVO_XML);

                    var xmlMemoryStream = new MemoryStream(nfseArquivoXmlBytes);

                    var zipArchiveEntry = zipArchive.CreateEntry(nfse.SEQ_ARQUIVOXML_NFSE + ".xml");

                    using (var streamWriter = zipArchiveEntry.Open())
                    {
                        xmlMemoryStream.CopyTo(streamWriter);
                    }
                }
            }
        }

        public async Task<byte[]> ExportaNfsePdfsAsync(IEnumerable<GeneratePdfRequest> generatePdfRequestList)
        {
            byte[] bytes = null;

            try
            {
                var fileName = @"wwwroot\NFSE\pdfs" + new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds() + ".zip";

                if (generatePdfRequestList.Any())
                {
                    var pdfList = await CreatePdfList(generatePdfRequestList);

                    CreatePdfZip(fileName, pdfList);

                    bytes = File.ReadAllBytes(fileName);

                    File.Delete(fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return bytes;
        }

        private byte[] BuildExcelByteArray(List<Nfse> nfseList)
        {
            var memoryStream = new MemoryStream();

            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var excelWorksheet = BuildExcelWorksheet(excelPackage, nfseList);

                memoryStream.Position = 0;
            }

            return memoryStream.ToArray();
        }

        private async Task<List<Nfse>> BuildNfseList(IEnumerable<int> notasServicoIdEnumerable)
        {
            var nfseList = new List<Nfse>();

            foreach (var id in notasServicoIdEnumerable)
            {
                var nfse = await _nfseServiceRepository.GetNfseByIdAsync(id);

                nfseList.Add(nfse);
            }

            return nfseList;
        }

        private ExcelWorksheet BuildExcelWorksheet(ExcelPackage excelPackage, List<Nfse> nfseList)
        {
            var excelWorksheet = excelPackage.Workbook.Worksheets.Add("NFse");

            excelWorksheet.Cells["A1"].LoadFromCollection(nfseList, true);

            excelWorksheet.Cells.AutoFitColumns();

            excelWorksheet.Row(1).Style.Font.Bold = true;

            excelWorksheet.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;

            excelWorksheet.Row(1).Style.Fill.BackgroundColor.SetColor(Color.MediumSeaGreen);

            excelPackage.Save();

            return excelWorksheet;
        }

        private async Task<List<Pdf>> CreatePdfList(IEnumerable<GeneratePdfRequest> generatePdfRequestList)
        {
            var pdfList = new List<Pdf>();

            foreach (var generatePdfRequest in generatePdfRequestList)
            {
                var generatePdf = generatePdfRequest.MapTo<GeneratePdfRequest, GeneratePdf>();
                var pdf = await _nfseServiceRepository.GeneratePdfAsync(generatePdf);
                pdfList.Add(pdf);
            }

            return pdfList;
        }

        private void CreatePdfZip(string fileName, List<Pdf> pdfList)
        {
            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Update))
                {
                    foreach (var pdf in pdfList)
                    {
                        var zipArchiveEntry = zipArchive.CreateEntry(pdf.NOMEPDF);

                        var pdfStream = new MemoryStream(pdf.ARQUIVOPDF);

                        using (var streamWriter = zipArchiveEntry.Open())
                        {
                            pdfStream.CopyTo(streamWriter);
                        }
                    }
                }
            }
        }
    }
}
