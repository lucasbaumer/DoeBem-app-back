using doeBem.Application.Interfaces;
using doeBem.Application.Services;
using doeBem.Core.Interfaces;
using doeBem.Infrastructure;
using doeBem.Infrastructure.Data;
using doeBem.Infrastructure.Repositories;
using doeBem.Presentation.Converters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicionar o contexto do banco de dados
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IDonorService, DonorService>();
builder.Services.AddScoped<IDonorRepository, DonorRepository>();
builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
builder.Services.AddScoped<IHospitalService, HospitalService>();

builder.Services.AddControllers();

// Adicionar outros serviços como o Entity Framework Core, etc.
var app = builder.Build();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
