using System.ComponentModel.DataAnnotations;

namespace PROY_INMOBILIARIA_malovini.Models;

public class DniValidatorModel
{
  [Required(ErrorMessage = "Campo incompleto.")]
  [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El DNI debe contener entre 7 y 8 números.")]
  public string Dni { get; set; }
}