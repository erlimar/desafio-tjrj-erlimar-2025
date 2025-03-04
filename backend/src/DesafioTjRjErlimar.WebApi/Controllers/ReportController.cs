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
    private readonly ManutencaoLivroAppService _manutencaoLivroAppService;
    private readonly TimeProvider _timeProvider;

    public ReportController(
        ManutencaoAutorAppService manutencaoAutorAppService,
        ManutencaoAssuntoAppService manutencaoAssuntoAppService,
        ManutencaoLivroAppService manutencaoLivroAppService,
        TimeProvider timeProvider)
    {
        _manutencaoAutorAppService = manutencaoAutorAppService
            ?? throw new ArgumentNullException(nameof(manutencaoAutorAppService));
        _manutencaoAssuntoAppService = manutencaoAssuntoAppService
            ?? throw new ArgumentNullException(nameof(manutencaoAssuntoAppService));
        _manutencaoLivroAppService = manutencaoLivroAppService
            ?? throw new ArgumentNullException(nameof(ManutencaoLivroAppService));
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
    [Route("livros")]
    public async Task<ActionResult> GerarRelatorioLivros()
    {
        var reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "CadastroLivros.frx");
        var reportData = await _manutencaoLivroAppService.ObterLivrosAsync();
        var report = new FastReport.Report();

        report.Report.Load(reportPath);
        report.Dictionary.RegisterBusinessObject(reportData.ToList(), "ReportDataSurce", 10, true);
        report.Prepare();

        var pdfExport = new PDFSimpleExport();

        MemoryStream ms = new();

        pdfExport.Export(report, ms);

        var enableRangeProcessing = true;

        ms.Seek(0, SeekOrigin.Begin);

        var nomeArquivo = $"relatorio-livros_{_timeProvider.GetUtcNow():yyyy-MM-ddTHH-mm-ss}.pdf";

        return File(ms, "application/odf", nomeArquivo, enableRangeProcessing);
    }

    // [HttpPost]
    // public ActionResult CriarRelatorioModelo()
    // {
    //     var path = AppDomain.CurrentDomain.BaseDirectory;
    //     var reportPath = Path.Combine(path, "Reports", "Modelo.frx");

    //     var data = GetItems();

    //     var report = new FastReport.Report();

    //     report.Dictionary.RegisterBusinessObject(data, "ReportDataSurce", 10, true);
    //     report.Report.Save(reportPath);

    //     return Ok(reportPath);
    // }
}