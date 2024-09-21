using System.Text.Json.Serialization;
using Ardalis.Specification;
using AutoGraphApi.Contexts;
using AutoGraphApi.Repositories;
using AutoGraphApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Регистрируем контроллеры с конфигурацией JSON
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        // Игнорировать циклические ссылки при сериализация
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка контекста базы данных
builder.Services.AddDbContext<AutoGraphContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AutoGraphConnectionString"),
            contextBuilder => contextBuilder.MigrationsAssembly(typeof(AutoGraphContext).Assembly.FullName))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Регистрация универсального репозитория
builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(GenericRepository<>));
// Регистрация универсального сервиса
builder.Services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopingCors",
        config =>
        {
            config.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Настройка файлов по умолчанию и статики
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("DevelopingCors");
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

await app.RunAsync();