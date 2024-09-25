using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Vendas.Domain.Interfaces.Repository;
using Vendas.Domain;
using Vendas.Repository;
using Microsoft.EntityFrameworkCore;
using Vendas.Domain.Interfaces.Service;

var builder = WebApplication.CreateBuilder(args);

// Configurar o Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Usar Serilog como o provedor de logging padr�
builder.Host.UseSerilog();

// Configura��o de SQLite e EF Core
builder.Services.AddDbContext<VendaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
// Configura��o dos reposit�rios e servi�os
builder.Services.AddScoped<IVendaService,VendaService>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();

// Adiciona controladores e API
builder.Services.AddControllers();

var app = builder.Build();

// Aplica as migrations no in�cio da aplica��o
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VendaContext>();
    dbContext.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Endpoints
app.MapControllers();

// Inicia a aplica��o
app.Run();


