using Application.Interfaces;
using Application.Services;
using BlazorApp.MVVM.Components;
using BlazorApp.MVVM.Helpers;
using BlazorApp.MVVM.Services;
using BlazorApp.MVVM.ViewModel;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.UnitOfWork;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

///Config - ConnectionString ------------------------------------------------------------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
//-------------------------------------------------------------------------------------------------------

///Congif - Interfaces_Services -----------------------------------------------------------------------------
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();// Servicio para el carrito de compras

builder.Services.AddBlazorDownloadFile(); // Para descargar archivos desde el navegador
QuestPDF.Settings.License = LicenseType.Community;//configurar licencia de QuestPDF



//Para injectar el httpClient en servicios, para llamados a APIs
builder.Services.AddHttpClient(); 

//Para el almacenamiento de datos protegidos en el navegador como si el usuario estuviera autenticado
builder.Services.AddScoped<ProtectedLocalStorage>();


//ViewModels para manejo de logica en MVVM
builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<ClientesViewModel>();
builder.Services.AddScoped<ProductosViewModel>();

builder.Services.AddScoped<CarritoViewModel>();// ViewModel para el carrito de compras
builder.Services.AddScoped<PagoViewModel>();// ViewModel para el proceso de checkout

//-------------------------------------------------------------------------------------------------------


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();


///Poblar BD al iniciar la app --------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();

    // Si quieres borrar y recrear la BD:
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Uncomment the next two lines if you want to drop and recreate the database on each run
    //dbContext.Database.EnsureDeleted();
    //dbContext.Database.EnsureCreated();

    // Si quieres poblar la BD con datos iniciales:
    await syncService.SyncClientsAsync();
    await syncService.SyncProductsAsync();
    await syncService.SyncUsersAsync();
}
//-------------------------------------------------------------------------------------------------------



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
