using Npgsql;
using Npgsql.PostgresTypes;
using NpgsqlTypes;
using System;

namespace EquipmentsAccounting.models
{
    internal class EquipmentsInfo
    {
        public int LocId { get; set; }
        public int GenId { get; set; }
        public string Type { get; set; }
        public string Characteristics { get; set; }
        public string NomenNum { get; set; }
        public string Subschet { get; set; }
        public string Provider { get; set; }
        public string DatePurchase { get; set; }
        public string Warranty { get; set; }
        public long Price { get; set; }
        public string SerialNum { get; set; }
        public string Status { get; set; }

        public EquipmentsInfo(int locId, int genId, string type, string characteristics, string nomenNum, string subschet, string provider, string datePurchase, string warrancy, int price, string serialNum, string status) 
        {
            this.LocId = locId;
            this.GenId = genId;
            this.Type = type;
            this.Characteristics = characteristics;
            this.NomenNum = nomenNum;
            this.Subschet = subschet;
            this.Provider = provider;
            this.DatePurchase = datePurchase;
            this.Warranty = warrancy;
            this.Price = price;
            this.SerialNum = serialNum;
            this.Status = status;
        }
    }
}
