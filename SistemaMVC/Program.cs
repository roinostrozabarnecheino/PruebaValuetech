var builder = WebApplication.CreateBuilder(args);

// Tus servicios...
builder.Services.AddHttpClient("ApiServicio", c =>
{
    c.BaseAddress = new Uri("https://localhost:7231/"); 
    c.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Region}/{action=Index}/{id?}");

app.Run();
