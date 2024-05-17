using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class DetailDevi
{
    public int IddetailDevis { get; set; }

    public int? Iddevis { get; set; }

    public int? Idtravaux { get; set; }

    public int? Idunite { get; set; }

    public double? Quantite { get; set; }

    public double? Pu { get; set; }

    public double? Total { get; set; }

    public double? Duree { get; set; }

    public string? Designation { get; set; }

    public virtual Devi? IddevisNavigation { get; set; }

    public virtual Travaux? IdtravauxNavigation { get; set; }

    public virtual Unite? IduniteNavigation { get; set; }
}
