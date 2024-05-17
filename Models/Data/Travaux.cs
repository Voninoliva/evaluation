using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class Travaux
{
    public int Idtravaux { get; set; }

    public int? Idtypetravaux { get; set; }

    public string? Numero { get; set; }

    public string? Designation { get; set; }

    public int? Idunite { get; set; }

    public double? Pu { get; set; }

    public DateOnly? Date { get; set; }

    public virtual ICollection<DetailDevi> DetailDevis { get; } = new List<DetailDevi>();

    public virtual TypeTravaux? IdtypetravauxNavigation { get; set; }

    public virtual Unite? IduniteNavigation { get; set; }

    public virtual ICollection<TravauxDesMaison> TravauxDesMaisons { get; } = new List<TravauxDesMaison>();
}
