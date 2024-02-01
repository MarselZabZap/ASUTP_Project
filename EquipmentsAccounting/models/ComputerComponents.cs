using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentsAccounting.models
{
    internal class ComputerComponents
    {
        public int Computer_id {  get; set; }
        public string CPU {  get; set; }
        public string Motherboard {  get; set; }
        public string Videocard { get; set; }
        public int RAM { get; set; }
        public int HDD { get; set; }
        public int SSD { get; set; }
        public string OperationSystem { get; set; }
        public string IpAdderss { get; set;}
    }
}
