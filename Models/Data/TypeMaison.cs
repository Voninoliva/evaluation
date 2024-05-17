using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class TypeMaison
{
    public int Idtypemaison { get; set; }

    public string? NomMaison { get; set; }

    public string? Descri { get; set; }

    public double? Duree { get; set; }

    public double? Surface { get; set; }

    public virtual ICollection<Devi> Devis { get; } = new List<Devi>();

    public virtual ICollection<TravauxDesMaison> TravauxDesMaisons { get; } = new List<TravauxDesMaison>();
}
