namespace ToDoList.Models
{
    public class MyList
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PdfUrl { get; set; }
        public DateTime Deadline{ get; set; }
    }
}
