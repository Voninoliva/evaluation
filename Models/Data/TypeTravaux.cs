using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class TypeTravaux
{
    public int Idtypetravaux { get; set; }

    public string? NomTravaux { get; set; }

    public string? NumeroType { get; set; }

    public virtual ICollection<Travaux> Travauxes { get; } = new List<Travaux>();
}
