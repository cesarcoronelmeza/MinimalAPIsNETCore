using BackEndApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BackEndApi.Services.Implementacion;
using BackEndApi.Services.Contrato;
using Microsoft.Data.SqlClient;
using AutoMapper;
using BackEndApi.DTOs;
using BackEndApi.Utilidades;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbMascotasContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")); 
} );

builder.Services.AddScoped<IMascotaService,MascotaService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


builder.Services.AddCors( options => {
    options.AddPolicy("NuevaPolitica", app => {
        app.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
      
    });
} );
 
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

#region API

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


app.MapGet("/mascota/List", async (
    IMascotaService _mascotaService,
    IMapper _mapper) =>
{
    var lstMascota = await _mascotaService.GetMascotaListAsync();
    var lstMascotaDTO = _mapper.Map<List<Mascota>>(lstMascota);

    if (lstMascotaDTO.Count > 0)
    {
        return Results.Ok(lstMascotaDTO);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapPost("/mascota/guardar", async (
    MascotaDTO model,
    IMascotaService _mascotaService,
    IMapper _mapper
    ) =>
{

    var mascota = _mapper.Map<Mascota>(model);
    var ressult = await _mascotaService.Add(mascota);

    if (ressult.IdMascota != 0)
        return Results.Ok(_mapper.Map<MascotaDTO>(ressult));
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);

});

app.MapPut("/mascota/modificar/{idMascota}", async (
    int idMascota,
    MascotaDTO model,
    IMascotaService _mascotaService,
    IMapper _mapper
    ) =>
{
    var _encontrado = await _mascotaService.GetMascotaById(idMascota);
    if (_encontrado == null)
    {
        return Results.NotFound();
    }

    var _mascota = _mapper.Map<Mascota>(model);

    _encontrado.IdMascota = _mascota.IdMascota;
    _encontrado.Nombre = _mascota.Nombre;
    _encontrado.Edad = _mascota.Edad;

    var _respuesta = await _mascotaService.Update(_encontrado);

    if (_respuesta)
        return Results.Ok(_mapper.Map<MascotaDTO>(_encontrado));
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);

});

app.MapDelete("/mascota/eliminar/{idMascota}", async (
    int idMascota,
    IMascotaService _mascotaService,
    IMapper _mapper) =>
{
    var _encontrado = await _mascotaService.GetMascotaById(idMascota);
    if (_encontrado == null)
    {
        return Results.NotFound();
    }

    var _respuesta = await _mascotaService.Delete(_encontrado);

    if (_respuesta)
        return Results.Ok();
    else
        return Results.StatusCode(StatusCodes.Status500InternalServerError);

});
#endregion

app.UseCors("NuevaPolitica");
 
app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
