using System;
using System.Collections.Generic;

namespace Proyecto_Notas.Models;

public partial class Estudiante
{
    public int IdEstudiante { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Correo { get; set; }

    public string? Celular { get; set; }

    public int? Edad { get; set; }

    public virtual ICollection<Nota> Nota { get; set; } = new List<Nota>();
}
