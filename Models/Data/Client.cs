using System;
using System.Collections.Generic;

namespace btp.Models.Data;

public partial class Client
{
    public int Idclient { get; set; }

    public string? Numero { get; set; }

    public virtual ICollection<Devi> Devis { get; } = new List<Devi>();
}
