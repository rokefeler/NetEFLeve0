using Microsoft.EntityFrameworkCore;
using proyectoef.Models;

namespace proyectoef;

public class TareasContext : DbContext {

  public DbSet<Categoria> Categorias {get;set;}
  public DbSet<Tarea> Tareas {get;set;}
  public TareasContext(DbContextOptions<TareasContext> options) : base(options) {}

  protected override void OnModelCreating(ModelBuilder modelBuilder) {

    List<Categoria> categoriasInit = new List<Categoria>() {
      new Categoria() { 
        CategoriaId = Guid.NewGuid(), 
        Nombre = "Actividades pendientes", 
        Peso = 10, 
        Description="Tareas pendientes",
        },
      new Categoria() { 
        CategoriaId = Guid.NewGuid(), 
        Nombre = "Actividades personales", 
        Peso = 10,
        Description="Tareas de ocio personales"
        },
      new Categoria() { 
        CategoriaId = Guid.NewGuid(), 
        Nombre = "Desarrollo Personal", 
        Peso = 80,
        Description="Tareas de crecimiento Personal"}
    };

    modelBuilder.Entity<Categoria>(categoria=>
    {
      categoria.ToTable("Category");
      categoria.HasKey(p=>p.CategoriaId);
      categoria.Property(p=>p.Nombre).HasMaxLength(100).IsRequired();
      categoria.Property(p=>p.Description).HasMaxLength(500);
      categoria.Property(p=>p.Peso);
      categoria.HasMany(p=>p.Tareas).WithOne(p=>p.Categoria).HasForeignKey(p=>p.CategoriaId);
      categoria.HasData(categoriasInit);
    });

    List<Tarea> tareasInit = new List<Tarea>() {
      new Tarea() { 
        TareaId = Guid.NewGuid(),
        Titulo = "Tarea 1", 
        Descripcion = "Pago de Servicios públicos", 
        FechaCreacion = DateTime.Now, 
        PrioridadTarea = Prioridad.Media, 
        CategoriaId = categoriasInit[0].CategoriaId,
        }, 
      new Tarea() { 
        TareaId = Guid.NewGuid(), 
        Titulo = "Tarea 2", 
        Descripcion = "Ver Pelicula", 
        FechaCreacion = DateTime.Now, 
        PrioridadTarea = Prioridad.Baja, 
        CategoriaId = categoriasInit[1].CategoriaId 
        },
      new Tarea() { 
        TareaId = Guid.NewGuid(), 
        Titulo = "Tarea 3", 
        Descripcion = "Terminar Diplomado", 
        FechaCreacion = DateTime.Now, 
        PrioridadTarea = Prioridad.Alta, 
        CategoriaId = categoriasInit[2].CategoriaId 
        }    
    };

    modelBuilder.Entity<Tarea>(tarea=>
    {
      tarea.ToTable("Task");
      tarea.HasKey(p=>p.TareaId);
      tarea.Property(p=>p.Titulo).HasMaxLength(200).IsRequired();
      tarea.Property(p=>p.Descripcion).HasMaxLength(500);
      tarea.Property(p=>p.PrioridadTarea).HasConversion<string>();
      tarea.HasOne(p=>p.Categoria).WithMany(p=>p.Tareas).HasForeignKey(p=>p.CategoriaId);
      tarea.Ignore(p=>p.Resumen);
      tarea.HasData(tareasInit);
    });
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Tarea>().Property(p=>p.PrioridadTarea).HasConversion<string>();
  }

}