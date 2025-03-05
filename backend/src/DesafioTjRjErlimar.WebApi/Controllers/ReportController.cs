using DesafioTjRjErlimar.Application.ManutencaoAssunto;
using DesafioTjRjErlimar.Application.ManutencaoAutor;
using DesafioTjRjErlimar.Application.ManutencaoLivro;

using FastReport.Export.PdfSimple;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTjRjErlimar.WebApi.Controllers;

[ApiController]
[Route("relatorios")]
public class ReportController : ControllerBase
{
    private readonly ManutencaoAutorAppService _manutencaoAutorAppService;
    private readonly ManutencaoAssuntoAppService _manutencaoAssuntoAppService;
    private readonly IManutencaoLivroAppRepository _manutencaoLivroAppRepository;
    private readonly TimeProvider _timeProvider;

    public ReportController(
        ManutencaoAutorAppService manutencaoAutorAppService,
        ManutencaoAssuntoAppService manutencaoAssuntoAppService,
        ManutencaoLivroAppService manutencaoLivroAppService,
        IManutencaoLivroAppRepository manutencaoLivroAppRepository,
        TimeProvider timeProvider)
    {
        _manutencaoAutorAppService = manutencaoAutorAppService
            ?? throw new ArgumentNullException(nameof(manutencaoAutorAppService));
        _manutencaoAssuntoAppService = manutencaoAssuntoAppService
            ?? throw new ArgumentNullException(nameof(manutencaoAssuntoAppService));
        _manutencaoLivroAppRepository = manutencaoLivroAppRepository
            ?? throw new ArgumentNullException(nameof(manutencaoLivroAppRepository));
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    [HttpGet]
    [Route("autores")]
    public async Task<ActionResult> GerarRelatorioAutores()
    {
        var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "CadastroAutores.frx");
        var reportData = await _manutencaoAutorAppService.ObterAutoresAsync();
        var report = new FastReport.Report();

        report.Report.Load(reportPath);
        report.Dictionary.RegisterBusinessObject(reportData.ToList(), "ReportDataSurce", 10, true);
        report.Prepare();

        var pdfExport = new PDFSimpleExport();

        MemoryStream ms = new();

        pdfExport.Export(report, ms);

        var enableRangeProcessing = true;

        ms.Seek(0, SeekOrigin.Begin);

        var nomeArquivo = $"relatorio-autores_{_timeProvider.GetUtcNow():yyyy-MM-ddTHH-mm-ss}.pdf";

        return File(ms, "application/odf", nomeArquivo, enableRangeProcessing);
    }

    [HttpGet]
    [Route("assuntos")]
    public async Task<ActionResult> GerarRelatorioAssuntos()
    {
        var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "CadastroAssuntos.frx");
        var reportData = await _manutencaoAssuntoAppService.ObterAssuntosAsync();
        var report = new FastReport.Report();

        report.Report.Load(reportPath);
        report.Dictionary.RegisterBusinessObject(reportData.ToList(), "ReportDataSurce", 10, true);
        report.Prepare();

        var pdfExport = new PDFSimpleExport();

        MemoryStream ms = new();

        pdfExport.Export(report, ms);

        var enableRangeProcessing = true;

        ms.Seek(0, SeekOrigin.Begin);

        var nomeArquivo = $"relatorio-assuntos_{_timeProvider.GetUtcNow():yyyy-MM-ddTHH-mm-ss}.pdf";

        return File(ms, "application/odf", nomeArquivo, enableRangeProcessing);
    }

    [HttpGet]
    [Route("consolidado")]
    public async Task<ActionResult> GerarRelatorioModelo()
    {
        var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "RelatorioConsolidadoPorAutor.frx");
        var reportData = await _manutencaoLivroAppRepository.ObterDadosDeLivrosParaRelatorioConsolidado();
        var report = new FastReport.Report();

        report.Report.Load(reportPath);
        report.Dictionary.RegisterBusinessObject(reportData.ToList(), "RelatorioConsolidadoPorAutor", 10, true);
        report.Prepare();

        var pdfExport = new PDFSimpleExport();

        MemoryStream ms = new();

        pdfExport.Export(report, ms);

        var enableRangeProcessing = true;

        ms.Seek(0, SeekOrigin.Begin);

        var nomeArquivo = $"relatorio-consolidado_{_timeProvider.GetUtcNow():yyyy-MM-ddTHH-mm-ss}.pdf";

        return File(ms, "application/odf", nomeArquivo, enableRangeProcessing);
    }
}