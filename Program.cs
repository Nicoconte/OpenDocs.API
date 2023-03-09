using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OpenDocs.API.Configurations.Extensions;
using OpenDocs.API.Configurations.Filters;
using OpenDocs.API.Data;
using OpenDocs.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetSection("ConnectionString").Value);
    options.UseLazyLoadingProxies();    
});

builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddTransient<IApplicationService, ApplicationService>();

builder.Services.AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining(typeof(Program));
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ApiExceptionFilterAttribute());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseCustomStaticFiles();


app.Run();
