using System;
using System.Collections.Generic;

namespace Cw8.Models;

public partial class Pccomponent
{
    public int Pcid { get; set; }

    public string ComponentCode { get; set; } = null!;

    public int Amount { get; set; }

    public virtual Component ComponentCodeNavigation { get; set; } = null!;

    public virtual Pc Pc { get; set; } = null!;
}
