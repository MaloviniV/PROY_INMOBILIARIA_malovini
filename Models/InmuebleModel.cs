using System.ComponentModel.DataAnnotations;

namespace PROY_INMOBILIARIA_malovini.Models;

public class InmuebleModel
{
  public int Id { get; set; }

  [StringLength(200)]
  public string? ImgPortada { get; set; }

  [Range(0, int.MaxValue)]
  public int Cupo { get; set; }

  [Required(ErrorMessage = "Ingrese la dirección.")]
  [StringLength(200)]
  public string Direccion { get; set; } = string.Empty;

  [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero.")]
  public decimal Precio { get; set; }

  public bool Estado { get; set; } = true;

  [Range(1, int.MaxValue, ErrorMessage = "Seleccione un propietario.")]
  public int IdPropietario { get; set; }

  [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de inmueble.")]
  public int IdTipo { get; set; }

  public string? NombreTipo { get; set; }
  public string? NombrePropietario { get; set; }
}
