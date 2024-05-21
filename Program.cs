using IndexCardBackendApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseNpgsql("Server=ep-bitter-bush-a5qztl34.us-east-2.aws.neon.tech;Database=CardApp;Username=neondb_owner;Password=Da5nA0CfZvQd;SSL Mode=Require;"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


// Configure CORS

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();
// In your Startup.cs Configure method
app.UseCors("AllowAnyOrigin");

app.Run();
