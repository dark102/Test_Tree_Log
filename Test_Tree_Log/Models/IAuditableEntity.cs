namespace Test_Tree_Log.Models
{
    public interface IAuditableEntity
    {
        DateTime createdDate { get; set; }
        DateTime updatedDate { get; set; }
    }
}
