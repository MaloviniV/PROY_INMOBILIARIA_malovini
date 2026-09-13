using System.ComponentModel.DataAnnotations;
namespace PROY_INMOBILIARIA_malovini.Models;

public class InquilinoModel
{
  public int IdPersona { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  public PersonaModel Persona { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 letras.")]
  public string Garante { get; set; }

  [Required(ErrorMessage = "Campo incompleto.")]
  [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 letras.")]
  public string Profesion { get; set; }

}