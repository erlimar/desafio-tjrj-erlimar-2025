using DesafioTjRjErlimar.Application.ManutencaoAutor;

using FastReport.Export.PdfSimple;

using Microsoft.AspNetCore.Mvc;

namespace DesafioTjRjErlimar.WebApi.Controllers;

[ApiController]
[Route("relatorios")]
public class ReportController : ControllerBase
{
    private readonly ManutencaoAutorAppService _manutencaoAutorAppService;

    public ReportController(ManutencaoAutorAppService manutencaoAutorAppService)
    {
        _manutencaoAutorAppService = manutencaoAutorAppService
            ?? throw new ArgumentNullException(nameof(manutencaoAutorAppService));
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

        return File(ms, "application/odf", "Autores.pdf", enableRangeProcessing);
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

public class AutorReportItem
{
    public int CodAutor { get; set; }
    public string? Nome { get; set; }
}