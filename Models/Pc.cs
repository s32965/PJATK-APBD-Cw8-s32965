using System;
using System.Collections.Generic;

namespace Cw8.Models;

public partial class Pc
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public float Weight { get; set; }

    public int Warranty { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<Pccomponent> Pccomponents { get; set; } = new List<Pccomponent>();
}
