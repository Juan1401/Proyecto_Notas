using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Notas.Models;

public partial class Nota
{
    public int IdNota { get; set; }

    public int? IdEstudiante { get; set; }

    public int? IdMateria { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Ingrese un número decimal válido.")]
    public decimal Nota1 { get; set; }

    public virtual Estudiante? IdEstudianteNavigation { get; set; }

    public virtual Materia? IdMateriaNavigation { get; set; }
}
