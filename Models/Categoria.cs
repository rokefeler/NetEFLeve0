using System.Text.Json.Serialization;

namespace proyectoef.Models;

public class Categoria{
  //[Key]
  public Guid CategoriaId { get; set; }
  
  //[Required(ErrorMessage = "El campo {0} es requerido")]
  //[MaxLength(150, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
  public String Nombre { get; set; }
  
  //[MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
  public String Description { get; set; }
  public int Peso { get; set; }
  
  [JsonIgnore]
  public virtual IEnumerable<Tarea> Tareas {get;set;}
}
