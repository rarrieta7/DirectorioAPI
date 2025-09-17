using DirectorioAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controladores
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositorios
builder.Services.AddSingleton<IPersonaRepository, PersonaRepository>();
builder.Services.AddSingleton<IFacturaRepository, FacturaRepository>();

// CORS (habilitado para cualquier origen)
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Activa CORS (antes de MapControllers)
app.UseCors("PermitirTodo");

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.RoutePrefix = "swagger"; // Asegura que quede en /swagger
    });
}

// Comentado para pruebas locales, así todo usa http:// y no hay error de certificado
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
