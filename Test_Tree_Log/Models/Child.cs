namespace Test_Tree_Log.Models
{
    public class Child: BaseModel
    {
        public string name { get; set; }
        public virtual List<Child>? children { get; set; }
        public virtual int treeid { get; set; }
    }
}
