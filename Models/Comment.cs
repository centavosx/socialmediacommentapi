using System.ComponentModel.DataAnnotations;

namespace TRYWEBAPI;

public class Comment
{

    public int Id {get; set;}


    public int AccountId {get; set;}
    public Account? Account{get; set;}

    public string Message {get; set;} = string.Empty;

    public DateTime Date {get; set;} = DateTime.Now;

    public int ReplyTo {get; set;} = int.MinValue;
    
}