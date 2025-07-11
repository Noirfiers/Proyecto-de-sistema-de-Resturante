using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_de_sistema_de_invetario.clases
{
    public abstract class EntidadBase
    {
        public string Id { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
