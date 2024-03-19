using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentsAccounting.models
{
    internal class StockEquipmentsInfo
    {
        public string Characteristics { get; set; }
        public int Count { get; set; }
        public int Sum { get; set; }
        public int Price { get; set; }

        public StockEquipmentsInfo(string characteristics, int count, int sum, int price)
        {
            Characteristics = characteristics;
            Count = count;
            Sum = sum;
            Price = price;
        }
    }
}
