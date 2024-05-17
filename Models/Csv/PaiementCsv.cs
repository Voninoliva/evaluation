using System;
using System.Collections.Generic;

namespace btp.Models.Csv;

public partial class PaiementCsv
{
    public int IdpaiementCsv { get; set; }

    public string? RefDevis { get; set; }

    public string? RefPaiement { get; set; }

    public DateOnly? DatePaiement { get; set; }

    public double? Montant { get; set; }
}
