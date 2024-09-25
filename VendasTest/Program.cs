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

// Usar Serilog como o provedor de logging padrã
builder.Host.UseSerilog();

// Configuração de SQLite e EF Core
builder.Services.AddDbContext<VendaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();
// Configuração dos repositórios e serviços
builder.Services.AddScoped<IVendaService,VendaService>();
builder.Services.AddScoped<IVendaRepository, VendaRepository>();

// Adiciona controladores e API
builder.Services.AddControllers();

var app = builder.Build();

// Aplica as migrations no início da aplicação
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

// Inicia a aplicação
app.Run();


