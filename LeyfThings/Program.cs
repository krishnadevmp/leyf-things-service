using LeyfThings.Exceptions;
using LeyfThings.LeyfThingsDB;
using LeyfThings.Middleware;
using LeyfThings.Models;
using LeyfThings.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new ErrorResponse
            {
                Type = "ValidationError",
                Status = StatusCodes.Status400BadRequest,
                Message = "One or more validation errors occurred.",
                TraceId = context.HttpContext.TraceIdentifier,
                Errors = errors
            };

            return new BadRequestObjectResult(response);
        };
    });

builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IGoalService, GoalService>();
builder.Services.AddScoped<IMileStoneService, MilestoneService>();
builder.Services.AddScoped<IOpenAIService, OpenAIService>();


var app = builder.Build();

// Global exception handling — must be first in the pipeline
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
