using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentsAccounting.models
{
    internal class IssueAct
    {
        public int locId { get; set; }
        public string dateIssue { get; set; }
        public string subschet { get; set; }
        public string eqName { get; set; }
        public string nomenNum { get; set; }
        public int requests { get; set; }
        public int released { get; set; }
        public int price { get; set; }
        public int sum { get; set; }
        public string manager_inic { get; set; }
        public string manager_full { get; set; }
        public string employeePosition { get; set; }
        public string employeeName_inic { get; set; }
        public string employeeName_full { get; set; }

        public IssueAct(int lociId, string dateIssue, string subschet, string eqName, string nomenNum, int requests, int released, int price, int sum, string manager_inic, string manager_full, string employeePosition, string employeeName_inic, string employeeName_full)
        {
            this.locId = lociId;
            this.dateIssue = dateIssue;
            this.subschet = subschet;
            this.eqName = eqName;
            this.nomenNum = nomenNum;
            this.requests = requests;
            this.released = released;
            this.price = price;
            this.sum = sum;
            this.manager_inic = manager_inic;
            this.manager_full = manager_full;
            this.employeePosition = employeePosition;
            this.employeeName_inic = employeeName_inic;
            this.employeeName_full = employeeName_full;
        }
    }
}
