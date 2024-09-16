namespace Test_Tree_Log.Models
{
    public class ExceptionData:BaseModel
    {
        public string exceptionType { get; set; }
        public string exceptionMessage { get; set; }
        public string stackTracert { get; set; }
        public virtual ExceptionData? inerExceptionData { get; set; }
    }
}
