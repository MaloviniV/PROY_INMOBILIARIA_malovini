using System.ComponentModel.DataAnnotations;

namespace PROY_INMOBILIARIA_malovini.Models;

public class PropietarioModel
{
  public int IdPersona { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [RegularExpression(@"^\d{22}$", ErrorMessage = "El CBU debe contener exactamente 22 números.")]
  public string Cbu { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [RegularExpression(@"^\d{11}$", ErrorMessage = "El CUIT debe contener exactamente 11 números.")]
  public string Cuit { get; set; }

  public bool Estado { get; set; }

  [Required]
  public PersonaModel Persona { get; set; }
}