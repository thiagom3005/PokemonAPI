using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokemonAPI.Domain.Interfaces;
using PokemonAPI.Domain.Validators;
using PokemonAPI.Repository.Contexts;
using PokemonAPI.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<PokemonContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PokemonDatabase")));
builder.Services.AddScoped<PokemonRequestValidator>();
builder.Services.AddScoped<IPokemonMasterService, PokemonMasterService>();
builder.Services.AddHttpClient<IPokeApiService, PokeApiService>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(uriString: configuration["PokeApiSettings:BaseUrl"] ?? "https://pokeapi.co/api/v2/");
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();