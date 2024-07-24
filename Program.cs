using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoef;
using proyectoef.Models;

var builder = WebApplication.CreateBuilder(args);


// builder.Services.AddDbContext<TareasContext>(p=>
//     p.UseInMemoryDatabase("TareasDB")
// );

builder.Services.AddDbContext<TareasContext>(p=>
    p.UseSqlServer(builder.Configuration.GetConnectionString("LocalDbConexion"))
);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/", () => "Hello World!");

app.MapGet("/dbconexion", async([FromServices] TareasContext dbcontext)=>{
    dbcontext.Database.EnsureCreated();
    return Results.Ok($"Base de datos en Memoria: {dbcontext.Database.IsInMemory()}");
});

app.MapGet("/api/tareas", async([FromServices] TareasContext dbcontext)=>{
    return Results.Ok(
        await dbcontext
        .Tareas.Include(t=>t.Categoria)
        .Where(x=>x.PrioridadTarea == Prioridad.Baja || x.PrioridadTarea == Prioridad.Media)
        .ToListAsync());
    });

app.MapPost("/api/tareas", async([FromServices] TareasContext dbcontext
    ,[FromBody] Tarea tarea)=>{
        tarea.TareaId = Guid.NewGuid();
        tarea.FechaCreacion = DateTime.Now;
        await dbcontext.AddAsync(tarea);
        //await dbcontext.Tareas.AddAsync(tarea); tambien funciona
        await dbcontext.SaveChangesAsync();
        return Results.Ok($"Tarea Creada: {tarea.Titulo}");

    });

app.MapPut("/api/tareas/{id}", async([FromServices] TareasContext dbcontext
    ,[FromBody] Tarea tarea
    ,[FromRoute] Guid id)=>{
        var tareaActual = await dbcontext.Tareas.FindAsync(id);
        if(tareaActual != null){
            tareaActual.CategoriaId = tarea.CategoriaId;
            tareaActual.Titulo = tarea.Titulo;
            tareaActual.Descripcion = tarea.Descripcion;
            tareaActual.PrioridadTarea = tarea.PrioridadTarea;
            await dbcontext.SaveChangesAsync();
            return Results.Ok($"Tarea Actualizada: {tareaActual.Titulo}");
        }
        return Results.NotFound();

    });


app.MapDelete("/api/tareas/{id}", 
    async([FromServices] TareasContext dbcontext,
    [FromRoute] Guid id)=>{
        var tareaActual = await dbcontext.Tareas.FindAsync(id);
        if(tareaActual != null){
            dbcontext.Remove(tareaActual);
            await dbcontext.SaveChangesAsync();
            return Results.Ok($"Tarea Eliminada: {tareaActual.Titulo}");
        }
        return Results.NotFound();
    });


app.Run();
