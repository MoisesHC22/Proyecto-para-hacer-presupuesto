using Presupuesto.Filters;
using Presupuesto.Infraestructura;
using Presupuesto.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//inyecciones 

builder.Services.AddLogging(logging =>
{ 
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
});

builder.Services.AddControllers(options => {
    options.Filters.Add<GlobalExceptionFilter>();
});

/// Registramos el filtro personalizado
builder.Services.AddScoped<GlobalExceptionFilter>();

/// Se registra el filtro dentro del controlador
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});


///configuramos todos los servicios en el middleware
builder.Services.AddTransient<ITiposCuentasServices, TiposCuentasServices>();
builder.Services.AddTransient<IUsuariosServices, ServiciosUsuarios>();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
