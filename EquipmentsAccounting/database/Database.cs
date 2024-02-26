using EquipmentsAccounting.model;
using EquipmentsAccounting.models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace EquipmentsAccounting.database
{
    internal class Database
    {

        //Параметры подключения к базе
        public NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(@"Server=localhost;Port=5432;User Id=postgres;Password=123;Database=EquipmentsAccounting;");
        }

        //Тестовое подключение к базе
        public void TestConnection()
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connected");
                }
            }
        }
        
        //Метод получния данных руководителя
        public Manager GetManager(string login)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                Manager manager = null;
                string query = String.Format(@"SELECT m.id, m.firstname, m.lastname, d.id, d.dep_name, m.login, m.password" +
                    "\nFROM managers m \n" +
                    "JOIN departments d ON d.id = m.dep_id \n" +
                    "WHERE m.login = '{0}'", login);
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    manager = new Manager
                        (
                            reader.GetInt16(0),
                            reader.GetString(1), 
                            reader.GetString(2),
                            reader.GetInt16(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6));
                }

                return manager;
            }
        }

        //Метод для получения DataTable на основе запроса
        public DataTable Query(string query)
        {
            using (NpgsqlConnection connection = GetConnection())
            {
                NpgsqlCommand cmd = new NpgsqlCommand(query, connection);
                connection.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                return dt;
            }
        }

        public List<EquipmentsInfo> GetEquipmentsInfoList()
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                string query = "SELECT * FROM all_eq_info";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                List<EquipmentsInfo> list = new List<EquipmentsInfo>();

                while (reader.Read())
                {
                    list.Add(new EquipmentsInfo(reader.GetInt16(0),
                                                reader.GetInt16(1),
                                                reader.GetString(2),
                                                reader.GetString(3),
                                                reader.GetString(4),
                                                reader.GetString(5),
                                                reader.GetString(6),
                                                reader.GetString(7),
                                                reader.GetString(8),
                                                reader.GetInt32(9),
                                                reader.GetString(10),
                                                reader.GetString(11)));
                }

                return list;
            }
        }

        //Метод получения всех данных об оборудовании
        public EquipmentsInfo GetEquipmentsInfoById(int eqId)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                string query = String.Format(@"SELECT * FROM all_eq_info_by_id({0})", eqId);
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                EquipmentsInfo equipmentsInfo = null;// = new EquipmentsInfo();
                while (reader.Read())
                {
                    equipmentsInfo = new EquipmentsInfo(reader.GetInt16(0),
                                                        reader.GetInt16(1),
                                                        reader.GetString(2),
                                                        reader.GetString(3),
                                                        reader.GetString(4),
                                                        reader.GetString(5),
                                                        reader.GetString(6),
                                                        reader.GetString(7),
                                                        reader.GetString(8),
                                                        reader.GetInt32(9),
                                                        reader.GetString(10),
                                                        reader.GetString(11));

                    /*equipmentsInfo.GenId = reader.GetInt16(1);
                    equipmentsInfo.Type = reader.GetString(2);
                    equipmentsInfo.Characteristics = reader.GetString(3);
                    equipmentsInfo.NomenNum = reader.GetString(4);
                    equipmentsInfo.Subschet = reader.GetString(5);
                    equipmentsInfo.Provider = reader.GetString(6);
                    equipmentsInfo.DatePurchase = reader.GetString(7);
                    equipmentsInfo.Warranty = reader.GetString(8);
                    equipmentsInfo.Price = reader.GetInt32(9);
                    equipmentsInfo.SerialNum = reader.GetString(10);
                    equipmentsInfo.Status = reader.GetString(11);*/
                }

                return equipmentsInfo;
            }
        }

        //Метод получения данных о компонентах компа
        public ComputerComponents GetComputerComponentsById(int computerId)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                string query = String.Format(@"SELECT computer_id, cpu, motherboard, video_card, ram, hdd, ssd, operation_system, ip_address
                                               FROM computer_components WHERE computer_id = {0}", computerId);
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader(); 
                ComputerComponents computerComponents = new ComputerComponents();
                while (reader.Read())
                {
                    computerComponents.Computer_id = reader.GetInt32(0);
                    computerComponents.CPU = reader.GetString(1);
                    computerComponents.Motherboard = reader.GetString(2);
                    computerComponents.Videocard = reader.GetString(3);
                    computerComponents.RAM = reader.GetInt32(4);
                    computerComponents.HDD = reader.GetInt32(5);
                    computerComponents.SSD = reader.GetInt32(6);
                    computerComponents.OperationSystem = reader.GetString(7);
                    computerComponents.IpAdderss = reader.GetString(8);
                    /*if (reader.GetString(8).Equals(null))
                    {
                        computerComponents.IpAdderss = "";
                    }
                    else
                    {
                        computerComponents.IpAdderss = reader.GetString(8);
                    }*/
                }
                return computerComponents;
            }
        }

        public List<Provider> GetProviders()
        {
            using(NpgsqlConnection conn = GetConnection())
            {
                string query = "SELECT * FROM providers";

                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();

                List<Provider> providers = new List<Provider>();

                while (reader.Read())
                {
                    providers.Add(new Provider(reader.GetInt32(0), reader.GetString(1)));
                }

                return providers;
            }
        }

        public void UpdateComputerData(int locId, int genId, string serialNumber, string provider, DateTime datePurchase, int price, string cpu,
            string motherboard, string videocard, int ram, int hdd, int ssd, string operationSystem)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                string updateAllEqInfo = "UPDATE loc_eq_acc SET serial_num = @serialNum WHERE id = @locId;" +
                    "UPDATE gen_eq_acc g SET provider = (SELECT p.id FROM providers p WHERE p.name = @provider)," +
                    "   date_purchase = @datePurchase, price = @price WHERE g.id = @genId;" +
                    "UPDATE computer_components SET cpu = @cpu, motherboard = @motherboard, video_card = @videocard, ram = @ram, hdd = @hdd, ssd = @ssd," +
                    "   operation_system = @operationSystem WHERE computer_id = @locId;";
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(updateAllEqInfo, conn))
                {
                    cmd.Parameters.AddWithValue("@locId", locId);
                    cmd.Parameters.AddWithValue("@genId", genId);
                    cmd.Parameters.AddWithValue("@serialNum", serialNumber);
                    cmd.Parameters.AddWithValue("@provider", provider);
                    cmd.Parameters.AddWithValue("@datePurchase", datePurchase);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@cpu", cpu);
                    cmd.Parameters.AddWithValue("@motherboard", motherboard);
                    cmd.Parameters.AddWithValue("@videocard", videocard);
                    cmd.Parameters.AddWithValue("@ram", ram);
                    cmd.Parameters.AddWithValue("@hdd", hdd);
                    cmd.Parameters.AddWithValue("@ssd", ssd);
                    cmd.Parameters.AddWithValue("@operationSystem", operationSystem);

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateIpAddress(int genId, string ipAddress)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                string query = "UPDATE computer_components SET ip_address = @ipAddress WHERE computer_id = @genId";
                conn.Open();

                using (NpgsqlCommand cmd = new NpgsqlCommand(query,conn))
                {
                    cmd.Parameters.AddWithValue("@genId", genId);
                    cmd.Parameters.AddWithValue("@ipAddress", ipAddress);

                    int rowsAffected = cmd.ExecuteNonQuery();
                }
            }
        }

        public List<GenEquipmentsAcc> GetGenEquipmentsAccList()
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                string query = "SELECT g.id, g.type_id, t.type_name, g.characteristics FROM gen_eq_acc g JOIN eq_types t ON t.id = g.type_id";
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query,conn);

                NpgsqlDataReader reader = cmd.ExecuteReader();

                List<GenEquipmentsAcc> genEquipments = new List<GenEquipmentsAcc>();

                while (reader.Read())
                {
                    genEquipments.Add(new GenEquipmentsAcc(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2),reader.GetString(3)));
                }

                return genEquipments;
            }
        }

        public void AddNewComputer(int genId, string serialNum, int depId, string cpu, string motherboard, string videocard, int ram, int hdd, int ssd, string opertaionSystem, string ipAddress)
        {
            string quety = String.Format("CALL add_new_computer({0},'{1}',{2},'{3}','{4}','{5}',{6},{7},{8},'{9}','{10}')", genId, serialNum, depId, cpu, motherboard, videocard, ram, hdd, ssd,
                                                                                                                opertaionSystem, ipAddress);
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                // добавление
                NpgsqlCommand cmd = new NpgsqlCommand(quety, conn);
                    int number = cmd.ExecuteNonQuery();
            }
        }

        public void AddOtherEquipments(int count, int genId, int depId)
        {
            string query = String.Format("CALL addOtherEquipments({0}, {1}, {2});", count, genId, depId);

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                int number = cmd.ExecuteNonQuery();
            }
        }

        public void AddOtherEquipments(string serialNum, int genId, int depId)
        {
            string query = String.Format("CALL addMonitorOrPrinter('{0}', {1}, {2});", serialNum, genId, depId);

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                int number = cmd.ExecuteNonQuery();
            }
        }

        public void SetOnMaintenance(int locId, string comments)
        {
            string query = String.Format("CALL addEquipMaintenance({0}, '{1}')", locId, comments);

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                int number = cmd.ExecuteNonQuery();
            }
        }

        public void TakeOffMaintenance(int locId)
        {
            string query = String.Format("CALL takeOffMaintenance({0})", locId);

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                int number = cmd.ExecuteNonQuery();
            }
        }

        /*public void WritrOffEquipment(int locId, string comments)
        {
            string query = String.Format("CALL writeOffEquipment({0}, '{1}')", locId, comments);

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                int number = cmd.ExecuteNonQuery();
            }
        }*/

        public IssueAct getAct(int locId, int request, int released)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                IssueAct act = null;
                string query = String.Format("SELECT * FROM getActInfo({0}, {1}, {2})", locId, request, released);
                NpgsqlCommand cmd = new NpgsqlCommand(query, conn);
                conn.Open();
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    act = new IssueAct
                        (
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetInt32(6),
                            reader.GetInt32(7),
                            reader.GetInt32(8),
                            reader.GetInt32(9),
                            reader.GetString(10),
                            reader.GetString(11),
                            reader.GetString(12),
                            reader.GetString(13),
                            reader.GetString(14)
                        );
                }
                return act;
            }
        }
    }
}
