using System.ComponentModel.DataAnnotations;

namespace PROY_INMOBILIARIA_malovini.Models;

public class ReservaModel
{
  public int Id { get; set; }

  [Range(1, int.MaxValue, ErrorMessage = "Seleccione un inquilino.")]
  public int IdInquilino { get; set; }

  [Range(1, int.MaxValue, ErrorMessage = "Seleccione un inmueble.")]
  public int IdInmueble { get; set; }

  [Required]
  public DateTime FechaDesde { get; set; }

  [Required]
  public DateTime FechaHasta { get; set; }

  [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
  public decimal Monto { get; set; }

  public string? Inquilino { get; set; }
  public string? DireccionInmueble { get; set; }

}
