using tl2_tp10_2023_iignac.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); //*** AGREGO PARA MANEJO DE SESION ***

var CadenaDeConexion = builder.Configuration.GetConnectionString("SqliteConexion")!.ToString(); //*** AGREGO PARA INYECTAR LA DB EN LOS REPOSITORIOS ***
builder.Services.AddSingleton<string>(CadenaDeConexion); //*** AGREGO PARA INYECTAR LA DB EN LOS REPOSITORIOS ***

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); //*** AGREGO PARA INYECTAR El REPOSITORIO USUARIO ***
builder.Services.AddScoped<ITableroRepository, TableroRepository>(); //*** AGREGO PARA INYECTAR El REPOSITORIO TABLERO ***
builder.Services.AddScoped<ITareaRepository, TareaRepository>(); //*** AGREGO PARA INYECTAR El REPOSITORIO TAREA ***

builder.Services.AddSession(options => //*** AGREGO PARA MANEJO DE SESION ***
{
    options.IdleTimeout = TimeSpan.FromSeconds(40); //tiempo de vida (en segundos) de la variable de sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.UseSession(); // *** AGREGO PARA MANEJO DE SESION ***
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
