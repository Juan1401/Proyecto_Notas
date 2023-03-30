using System;
using System.Collections.Generic;

namespace Proyecto_Notas.Models;

public partial class Materia
{
    public int IdMateria { get; set; }

    public string? NombreMateria { get; set; }

    //Como estaba
    //public virtual ICollection<Nota> Nota { get; } = new List<Nota>();


    public virtual ICollection<Nota> Nota { get; set; }
}
