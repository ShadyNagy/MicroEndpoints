using MicroEndpoints.EndpointApp.DataAccess;
using MicroEndpoints.EndpointApp.DomainModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MicroEndpoints.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite("Data Source=database.sqlite"));
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddMicroEndpoints(Assembly.GetAssembly(typeof(Program)));

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseMicroEndpoints();

app.Run();
