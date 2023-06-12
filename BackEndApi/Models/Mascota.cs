using System;
using System.Collections.Generic;

namespace BackEndApi.Models;

public partial class Mascota
{
    public int IdMascota { get; set; }

    public string Nombre { get; set; } = null!;

    public int Edad { get; set; }

    public string Descripcion { get; set; } = null!;
}
