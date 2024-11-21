var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        // Altere aqui para permitir apenas a origem específica que você deseja
        policy.WithOrigins("http://127.0.0.1:5500") // Sua origem, por exemplo, o localhost com a porta 5500
              .AllowAnyMethod()    // Permite qualquer método HTTP (GET, POST, etc)
              .AllowAnyHeader();   // Permite qualquer cabeçalho
    });
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS com a política configurada
app.UseCors("AllowSpecificOrigin");  // Isso habilita o CORS usando a política "AllowSpecificOrigin"

app.UseAuthorization();

app.MapControllers();

app.Run();  // Aqui é a última linha, o parêntese está fechado corretamente.
