using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentsAccounting.models
{
    internal class Departament
    {
        public int id {  get; set; }
        public string name { get; set; }

        public Departament(int id, string name) 
        {
            this.id = id;
            this.name = name;
        }
    }
}
