using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentsAccounting.model
{
    internal class Manager
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Dep_id { get; set; }   
        public string Dep_name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Manager(int Id, string Firstname, string Lastname, int Dep_id, string Dep_name, string Login, string Password) 
        {
            this.Id = Id;
            this.Firstname = Firstname;
            this.Lastname = Lastname;
            this.Dep_id = Dep_id;
            this.Dep_name = Dep_name;
            this.Login = Login;
            this.Password = Password;
        }
    }
}
