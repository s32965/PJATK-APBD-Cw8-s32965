using System;
using System.Collections.Generic;

namespace Cw8.Models;

public partial class Component
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int ComponentManufacturersId { get; set; }

    public int ComponentTypesId { get; set; }

    public virtual ComponentManufacturer ComponentManufacturers { get; set; } = null!;

    public virtual ComponentType ComponentTypes { get; set; } = null!;

    public virtual ICollection<Pccomponent> Pccomponents { get; set; } = new List<Pccomponent>();
}
