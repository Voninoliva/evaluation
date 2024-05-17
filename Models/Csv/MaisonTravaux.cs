using System;
using System.Collections.Generic;

namespace btp.Models.Csv;

public partial class MaisonTravaux
{
    public int IdmaisonTravaux { get; set; }

    public string? TypeMaison { get; set; }

    public string? Description { get; set; }

    public double? Surface { get; set; }

    public string? CodeTravaux { get; set; }

    public string? TypeTravaux { get; set; }

    public string? Unite { get; set; }

    public double? Pu { get; set; }

    public double? Quantite { get; set; }

    public double? DureeTravaux { get; set; }
}
