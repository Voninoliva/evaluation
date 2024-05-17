using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class TravauxDesMaison
{
    public int Idtravauxdesmaisons { get; set; }

    public int? Idtypemaison { get; set; }

    public int? Idtravaux { get; set; }

    public int? Idunite { get; set; }

    public double? Quantite { get; set; }

    public double? Pu { get; set; }

    public double? Total { get; set; }

    public double? Duree { get; set; }

    public string? Designation { get; set; }

    public virtual Travaux? IdtravauxNavigation { get; set; }

    public virtual TypeMaison? IdtypemaisonNavigation { get; set; }

    public virtual Unite? IduniteNavigation { get; set; }
}
