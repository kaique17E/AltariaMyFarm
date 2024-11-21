using DsiVendas.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Session; // Adicionado para utilizar sessão
using Microsoft.Extensions.Caching.Memory; // Adicionado para cache de sessão

var builder = WebApplication.CreateBuilder(args);

// Configura o contexto do banco de dados com SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Filename=banco.db"));

// Adiciona o serviço de autenticação (garanta que AuthService esteja implementado corretamente)
builder.Services.AddScoped<IAuthService, AuthService>();

// Adiciona os serviços de controle e visão
builder.Services.AddControllersWithViews();

// Configuração da cultura para "pt-BR"
var supportedCultures = new[] { new CultureInfo("pt-BR") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// Configuração de localização
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configuração do CORS (permitindo qualquer origem)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Permite qualquer origem (domínio)
              .AllowAnyMethod()    // Permite qualquer método HTTP (GET, POST, PUT, etc.)
              .AllowAnyHeader();   // Permite qualquer cabeçalho
    });
});

// Configuração de sessão
builder.Services.AddDistributedMemoryCache(); // Adiciona o cache necessário para sessões
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Define o tempo de expiração da sessão
    options.Cookie.HttpOnly = true; // Garante que o cookie da sessão seja acessível apenas pelo servidor
    options.Cookie.IsEssential = true; // Define o cookie como essencial para a aplicação
});

var app = builder.Build();

// Aplica o CORS no pipeline
app.UseCors("AllowAll");  // Aplica a política de CORS

// Configura o pipeline de requisição
app.UseRequestLocalization(localizationOptions);

// Habilita a sessão no pipeline de requisição
app.UseSession(); // Adiciona o middleware de sessão

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // Certifique-se de usar a autenticação
app.UseAuthorization();

// Mapeamento das rotas de controlador
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
