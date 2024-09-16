using System.ComponentModel.DataAnnotations;

namespace Test_Tree_Log.Models
{
    public class BaseModel:IAuditableEntity
    {
        public int id { get; set; }

        public DateTime updatedDate { get; set; }

        public DateTime createdDate { get; set; }
    }
}
