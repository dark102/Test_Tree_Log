namespace Test_Tree_Log.Models
{
    public class ExceptionJournal: BaseModel
    {
        public string eventID { get; set; }
        public string pathRequest { get; set; }
        public string? paramRequest { get; set; }
        public string? bodyRequest { get; set; }
        public virtual ExceptionData exceptionData { get; set; }
    }
    public class ViewsModelExceptionJournal
    {
        public int skip { get; set; }
        public int count { get; set; }
        public List<item> items { get; set; }
    }
    public class item
    {
        public int id { get; set; }
        public string eventID { get; set; }
        public string createdDate { get; set; }
    }
    public class ViewsModelExceptionJournalDetails
    {
        public string text { get; set; }
        public int id { get; set; }
        public string eventID { get; set; }
        public string createdDate { get; set; }
    }
}
