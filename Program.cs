using Microsoft.EntityFrameworkCore;
using TreeApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавляем DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем поддержку контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Маппинг контроллеров
app.MapControllers();

app.Run();