using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionUsuarios.Models;

[Table("usuarios")]
public partial class Usuario
{
    [Key]
    [Column("id")]
    public required int Id { get; set; }

    [Column("nombre")]
    [MaxLength(45, ErrorMessage = "El campo nombre no debe estar vacío.")]
    public required string Nombre { get; set; }

    [Column("apellido")]
    [MaxLength(45, ErrorMessage = "El campo apellido no debe estar vacío.")]
    public required string Apellido { get; set; }

    [Column("fecha_nacimiento")]
    public required DateOnly FechaNacimiento { get; set; }
}