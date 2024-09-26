using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Build application
var app = builder.Build();

// Configure the HTTP request pipeline.
// [SEM] This section is known as middleware

// app.UseHttpsRedirection();
// app.UseAuthorization();

app.MapControllers();

app.Run();
