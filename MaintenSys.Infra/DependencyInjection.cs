using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Interfaces;
using MaintenSys.Infra.Identity;
using MaintenSys.Infra.Pdf;
using MaintenSys.Infra.Persistence;
using MaintenSys.Infra.Persistence.Repositories;
using MaintenSys.Infra.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace MaintenSys.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddDbContext<MaintenSysDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IAparelhoRepository, AparelhoRepository>();
        services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddSingleton<IPasswordHasher, PasswordHasherService>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPdfService, QuestPdfOrdemServicoPdfService>();
        services.AddSingleton<IArquivoStorageService, LocalFileStorageService>();

        return services;
    }
}
