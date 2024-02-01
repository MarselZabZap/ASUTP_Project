using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentsAccounting.models
{
    internal class GenEquipmentsAcc
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; } 
        
        public GenEquipmentsAcc(int id, int typeId, string typeName, string name) 
        {
            Id = id;
            TypeId = typeId;
            TypeName = typeName;
            Name = name;
        }
    }
}
