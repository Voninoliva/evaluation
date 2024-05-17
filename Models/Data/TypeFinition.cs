using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class TypeFinition
{
    public int Idtypefinition { get; set; }

    public string? NomFinition { get; set; }

    public double? AugmentationPourcentage { get; set; }

    public virtual ICollection<Devi> Devis { get; } = new List<Devi>();
}
