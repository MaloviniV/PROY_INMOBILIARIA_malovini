using System.ComponentModel.DataAnnotations;

namespace PROY_INMOBILIARIA_malovini.Models;

public class PersonaModel
{
  public int Id { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 letras.")]
  public string Apellido { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 letras.")]
  public string Nombre { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe contener entre 7 y 8 números.")]
  public string Dni { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [Phone(ErrorMessage = "Ingrese un teléfono valido")]
  public string Telefono { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [EmailAddress(ErrorMessage = "Ingrese un mail valido.")]
  public string Mail { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "Ingrese una direccion valida debe tener entre 3 y 50 caracteres.")]
  public string Direccion { get; set; }
}