var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        // Altere aqui para permitir apenas a origem espec�fica que voc� deseja
        policy.WithOrigins("http://127.0.0.1:5500") // Sua origem, por exemplo, o localhost com a porta 5500
              .AllowAnyMethod()    // Permite qualquer m�todo HTTP (GET, POST, etc)
              .AllowAnyHeader();   // Permite qualquer cabe�alho
    });
});

// Configura��o do Swagger
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

// Habilitar CORS com a pol�tica configurada
app.UseCors("AllowSpecificOrigin");  // Isso habilita o CORS usando a pol�tica "AllowSpecificOrigin"

app.UseAuthorization();

app.MapControllers();

app.Run();  // Aqui � a �ltima linha, o par�ntese est� fechado corretamente.
