using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using UserService.Application;
using UserService.Domain.Entities;
using UserService.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Definindo a política de CORS
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container
builder.Services.AddControllers();

// Configurando o Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "User Service API",
            Version = "v1",
            Description = "API para gerenciamento de usuários no User Service",
        }
    )
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var app = builder.Build();

// Ativando o middleware do Swagger e SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Service API V1"));
}

// Habilitar redirecionamento HTTPS
app.UseHttpsRedirection();

// Aplicando a política de CORS
app.UseCors(myAllowSpecificOrigins);

// Middlewares de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Mapeamento dos controllers
app.MapControllers();

// Executar a aplicação
app.Run();
