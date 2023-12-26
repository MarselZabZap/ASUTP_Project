using EquipmentsAccounting.model;
using EquipmentsAccounting.models;

namespace EquipmentsAccounting
{
    internal class Singleton
    {
        public static int EQ_ID, EM_ID = -1;
        public static string START_DATE, END_DATE;


        public static Manager MANAGER;
        public static Employee EMPLOYEE;
        public static GenEquipmentsAcc GEN_EQUIPMENT;
        public static EquipmentsInfo EQUIPMENS_INFO;
    }
}
