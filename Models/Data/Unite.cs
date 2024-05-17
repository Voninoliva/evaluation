using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class Unite
{
    public int Idunite { get; set; }

    public string? NomUnite { get; set; }

    public virtual ICollection<DetailDevi> DetailDevis { get; } = new List<DetailDevi>();

    public virtual ICollection<TravauxDesMaison> TravauxDesMaisons { get; } = new List<TravauxDesMaison>();

    public virtual ICollection<Travaux> Travauxes { get; } = new List<Travaux>();
}
