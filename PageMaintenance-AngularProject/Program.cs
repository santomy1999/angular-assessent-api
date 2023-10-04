using Microsoft.EntityFrameworkCore;
using PageMaintenance_AngularProject.Data;
using PageMaintenance_AngularProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PolAdminSysContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Dependency Injection
builder.Services.AddScoped<ITableInterface, TableRepository>();
builder.Services.AddScoped<IFormInterface, FormRepository>();

builder.Services.AddCors(option =>

{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
