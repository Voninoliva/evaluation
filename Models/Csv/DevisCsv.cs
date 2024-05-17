using System;
using System.Collections.Generic;

namespace btp.Models.Csv;

public partial class DevisCsv
{
    public int IdDevisCsv { get; set; }

    public string? Client { get; set; }

    public string? RefDevis { get; set; }

    public string? TypeMaison { get; set; }

    public string? Finition { get; set; }

    public double? TauxFinition { get; set; }

    public DateOnly? DateDevis { get; set; }

    public DateOnly? DateDebut { get; set; }

    public string? Lieu { get; set; }
}
