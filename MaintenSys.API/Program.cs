using System.Text;
using MaintenSys.API.Middleware;
using MaintenSys.API.Services;
using MaintenSys.Application;
using MaintenSys.Application.Interfaces;
using MaintenSys.Domain.Entities;
using MaintenSys.Domain.Enums;
using MaintenSys.Domain.Interfaces;
using MaintenSys.Infra;
using MaintenSys.Infra.Identity;
using MaintenSys.Infra.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace MaintenSys.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddApplication();
        builder.Services.AddInfra(builder.Configuration);
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()!;

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });

        builder.Services.AddAuthorization();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MaintenSys API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Informe o token JWT no formato: Bearer {seu token}"
            });

            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("Bearer", document), [] }
            });
        });

        var app = builder.Build();

        await AplicarMigracoesESeedAsync(app);

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        await app.RunAsync();
    }

    private static async Task AplicarMigracoesESeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var provider = scope.ServiceProvider;

        var dbContext = provider.GetRequiredService<MaintenSysDbContext>();
        await dbContext.Database.MigrateAsync();

        var configuration = provider.GetRequiredService<IConfiguration>();
        var adminEmail = configuration["AdminPadrao:Email"];
        if (string.IsNullOrWhiteSpace(adminEmail))
            return;

        var usuarioRepository = provider.GetRequiredService<IUsuarioRepository>();
        var existente = await usuarioRepository.ObterPorEmailAsync(adminEmail);
        if (existente is not null)
            return;

        var passwordHasher = provider.GetRequiredService<IPasswordHasher>();
        var unitOfWork = provider.GetRequiredService<IUnitOfWork>();

        var nome = configuration["AdminPadrao:Nome"] ?? "Administrador";
        var senha = configuration["AdminPadrao:Senha"] ?? "Admin@123";

        var admin = new Usuario(nome, adminEmail, passwordHasher.Hash(senha), TipoUsuario.Administrador);
        await usuarioRepository.AdicionarAsync(admin);
        await unitOfWork.SalvarAlteracoesAsync();
    }
}
