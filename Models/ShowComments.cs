namespace TRYWEBAPI.Models
{
    public class ShowComments
    {
        public int Id {get; set;}
        public string? Name {get; set;}
        public string? Username {get; set;}
        public string? Message {get; set;}
        public DateTime? Date {get; set;} 
        public List<ShowComments>? Replies {get; set;}

        public int ReplyToId {get; set;}
    }
}