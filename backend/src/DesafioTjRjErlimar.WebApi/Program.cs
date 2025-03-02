using DesafioTjRjErlimar.Application.ManutencaoDeLivros;
using DesafioTjRjErlimar.DatabaseAdapter;
using DesafioTjRjErlimar.DatabaseAdapter.Repositories;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        #region Configurações do ASP.NET
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddProblemDetails();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        #endregion

        #region Configuração de acesso ao banco
        var connectionString = builder.Configuration.GetConnectionString("DesafioTjRjErlimar")
            ?? throw new InvalidOperationException("Connection string 'DesafioTjRjErlimar' não configurada");

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        #endregion

        #region Configurações de serviços de aplicação
        builder.Services
            .AddScoped<IManutencaoLivroAppRepository, ManutencaoLivroRelationalAppRepository>()
            .AddScoped<ManutencaoLivroAppService>();
        #endregion

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}