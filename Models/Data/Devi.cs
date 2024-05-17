using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class Devi
{
    public int Iddevis { get; set; }

    public int? Idclient { get; set; }

    public int? Idtypemaison { get; set; }

    public int? Idtypefinition { get; set; }

    public double? TauxFinition { get; set; }

    public double? MontantTotalTravaux { get; set; }

    public DateOnly? DateInsertion { get; set; }

    public DateOnly? DateDebutTravaux { get; set; }

    public double? MontantTotal { get; set; }

    public double? Duree { get; set; }

    public string? RefDevis { get; set; }

    public string? Lieu { get; set; }

    public virtual ICollection<DetailDevi> DetailDevis { get; } = new List<DetailDevi>();

    public virtual Client? IdclientNavigation { get; set; }

    public virtual TypeFinition? IdtypefinitionNavigation { get; set; }

    public virtual TypeMaison? IdtypemaisonNavigation { get; set; }

    public virtual ICollection<PaiementDevi> PaiementDevis { get; } = new List<PaiementDevi>();
}
