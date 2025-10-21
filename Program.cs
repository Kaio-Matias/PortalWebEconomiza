using Microsoft.EntityFrameworkCore;
using PortalWebEconomiza.Config;
using PortalWebEconomiza.Data;
using PortalWebEconomiza.Services;

var builder = WebApplication.CreateBuilder(args);

// --- INÍCIO DAS MODIFICAÇÕES ---

// 1. Carregar a Connection String do appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. Carregar a Configuração da API do appsettings.json
var apiConfigSection = builder.Configuration.GetSection("ApiConfig");
var apiConfig = apiConfigSection.Get<ApiConfig>()
    ?? throw new InvalidOperationException("ApiConfig não está configurado corretamente no appsettings.json.");

// 3. Registar o ApiConfig para ser injetado via IOptions<ApiConfig>
builder.Services.Configure<ApiConfig>(apiConfigSection);

// 4. Registar o DbContext (SQL Server)
builder.Services.AddDbContext<EconomizaDbContext>(options =>
    options.UseSqlServer(connectionString));

// 5. Registar o SefazApiClient usando AddHttpClient
//    Isto regista 'SefazApiClient' e injeta um 'HttpClient' configurado nele
builder.Services.AddHttpClient<SefazApiClient>(client =>
{
    client.BaseAddress = new Uri(apiConfig.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(60); // Timeout de 60 segundos
});

// 6. Registar o ConsultaRepository (como Scoped)
builder.Services.AddScoped<ConsultaRepository>();

// --- FIM DAS MODIFICAÇÕES ---

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();