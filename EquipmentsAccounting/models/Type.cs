namespace EquipmentsAccounting.models
{
    internal class Type
    {

        public int Id { get; set; } 
        public string TypeName { get; set; }

        public Type(int id, string typeName)
        {
            this.Id = id;
            this.TypeName = typeName; 
        } 
    }
}
