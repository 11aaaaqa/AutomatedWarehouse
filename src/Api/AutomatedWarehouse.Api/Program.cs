using AutomatedWarehouse.Api.Domain.Models;
using AutomatedWarehouse.Api.Infrastructure.Database;
using AutomatedWarehouse.Api.Infrastructure.Services.Guide_services;
using AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Document_services;
using AutomatedWarehouse.Api.Infrastructure.Services.Receipt_services.Resource_services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(x
    => x.UseNpgsql(builder.Configuration["Database:ConnectionString"]));

builder.Services.AddTransient<IGuideService<Resource>, ResourceService>();
builder.Services.AddTransient<IGuideService<MeasurementUnit>, MeasurementUnitService>();
builder.Services.AddTransient<IReceiptResourceService, ReceiptResourceService>();
builder.Services.AddTransient<IReceiptDocumentService, ReceiptDocumentService>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
