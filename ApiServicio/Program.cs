var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddScoped<AccesoDatos.RegionServicio>();

builder.Services.AddCors(p => p.AddPolicy("PermitirMVC", pol =>
{
    pol.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();
app.UseCors("PermitirMVC"); 
app.UseAuthorization();
app.MapControllers(); 

app.Run();
