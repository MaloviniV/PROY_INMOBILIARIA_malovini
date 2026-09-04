using System.ComponentModel.DataAnnotations;

namespace PROY_INMOBILIARIA_malovini.Models;

public class TipoInmuebleModel
{
  public int Id { get; set; }

  [Required(ErrorMessage = "Ingrese el nombre del tipo de inmueble.")]
  [StringLength(50)]
  public string Nombre { get; set; } = string.Empty;
}
