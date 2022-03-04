namespace TRYWEBAPI.Models
{
    public class SearchComments
    {
        public int? Id {get; set;}

        public int? AccountId {get; set;}
        public string? Name {get; set;}
        public string? Username {get; set;}
        public string? Message {get; set;}


        public DateTime? Date {get; set;} 
        public bool? All {get; set;}
    }
}