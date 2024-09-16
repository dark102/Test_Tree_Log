namespace Test_Tree_Log.Models
{
    public class Tree:BaseModel
    {
        public string name { get; set; }
        public virtual List<Child> children { get; set; }
    }

}
