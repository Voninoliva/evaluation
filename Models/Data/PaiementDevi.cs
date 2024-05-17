using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class PaiementDevi
{
    public int Idpaiementdevis { get; set; }

    public int? Iddevis { get; set; }

    public DateOnly? DateInsertion { get; set; }

    public DateOnly? DatePrevue { get; set; }

    public DateOnly? DatePaiement { get; set; }

    public double? Montant { get; set; }

    public string? RefPaiement { get; set; }

    public virtual Devi? IddevisNavigation { get; set; }
}
