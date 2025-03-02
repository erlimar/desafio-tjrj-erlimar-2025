
using DesafioTjRjErlimar.DatabaseAdapter;

using Microsoft.EntityFrameworkCore;

namespace DesafioTjRjErlimar.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        #region Configuração de acesso ao banco
        var connectionString = builder.Configuration.GetConnectionString("DesafioTjRjErlimar")
            ?? throw new InvalidOperationException("Connection string 'DesafioTjRjErlimar' não configurada");

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        #endregion

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}