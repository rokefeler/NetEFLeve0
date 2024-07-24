
namespace proyectoef.Models;

public class Tarea
{
    //[Key]
    public Guid TareaId {get;set;}
    //[ForeignKey("Categoria")]
    public Guid CategoriaId {get;set;}

    //[Required(ErrorMessage = "El campo {0} es requerido")]
    //[MaxLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string Titulo {get;set;}

    //[MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string Descripcion {get;set;}

    public Prioridad PrioridadTarea {get;set;}

    public DateTime FechaCreacion {get;set;}
    
    public virtual Categoria Categoria {get;set;}

    public string Resumen {get;set;}
}

public enum Prioridad
{
    Baja,
    Media,
    Alta
}