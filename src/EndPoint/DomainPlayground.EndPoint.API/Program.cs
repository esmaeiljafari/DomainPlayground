using DomainPlayground.Core.Application;
using DomainPlayground.Core.Application.Common.Interfaces;
using DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.CreateRole;
using DomainPlayground.Infrastructure;
using DomainPlayground.Infrastructure.Identity;
using DomainPlayground.Infrastructure.Identity.Contexts;
using DomainPlayground.Infrastructure.Persistence.Commands;
using DomainPlayground.Infrastructure.Persistence.Commands.Authorization.Services;
using DomainPlayground.Infrastructure.Persistence.Commands.Common.Contexts;
using DomainPlayground.Infrastructure.Persistence.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "DomainPlayground API";
        document.Info.Version = "v1";

        var securityScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "توکن JWT خود را وارد کنید"
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
        document.Components.SecuritySchemes["Bearer"] = securityScheme;

        var schemeReference = new OpenApiSecuritySchemeReference("Bearer", document);
        var securityRequirement = new OpenApiSecurityRequirement
        {
            [schemeReference] = new List<string>()
        };

        document.Security ??= new List<OpenApiSecurityRequirement>();
        document.Security.Add(securityRequirement);

        return Task.CompletedTask;
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddApplication();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddCommandPersistence(builder.Configuration);
builder.Services.AddQueryPersistence(builder.Configuration);
builder.Services.AddInfrastructure();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, DomainPlayground.EndPoint.API.Services.CurrentUser>();

var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSection["Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret is missing in configuration.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(secretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseRequestLocalization();

var supported = new[] { "fa", "en" };
app.UseRequestLocalization(o => o
    .SetDefaultCulture("fa")
    .AddSupportedCultures(supported)
    .AddSupportedUICultures(supported));

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "DomainPlayground API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var identityDb = services.GetRequiredService<IdentityAppDbContext>();
    await identityDb.Database.MigrateAsync();

    var commandDb = services.GetRequiredService<ApplicationCommandDbContext>();
    await commandDb.Database.MigrateAsync();

    var synchronizer = services.GetRequiredService<PermissionSynchronizer>();
    await synchronizer.SyncAsync(typeof(CreateRoleCommand).Assembly);

    var seeder = services.GetRequiredService<AuthorizationSeeder>();
    await seeder.SeedAsync(typeof(CreateRoleCommand).Assembly);
}

app.Run();