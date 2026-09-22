using MyFinanceWeb.Infra;
using MyFinanceWeb.Infra.Database;
using MyFinanceWeb.Service.Interfaces;
using MyFinanceWeb.Service;
using MyFinanceWeb.Infra.Interfaces;
using MyFinanceWeb.Infra.Repositories;
using Npgsql;

DotNetEnv.Env.NoClobber().TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

if (!builder.Environment.IsEnvironment("Testing"))
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "A connection string 'DefaultConnection' nao foi configurada."
        );
    var databasePassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD")
        ?? throw new InvalidOperationException(
            "A variavel de ambiente 'POSTGRES_PASSWORD' nao foi configurada."
        );
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString)
    {
        Password = databasePassword
    };

    builder.Services.AddMyFinanceDbContext(
        new PostgreSqlDatabaseStrategy(connectionStringBuilder.ConnectionString)
    );
}

builder.Services.AddScoped<IPlanoContaService, PlanoContaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();

builder.Services.AddScoped<IPlanoContaRepository, PlanoContaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
