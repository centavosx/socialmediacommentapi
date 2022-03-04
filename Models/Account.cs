using System.ComponentModel.DataAnnotations;

namespace TRYWEBAPI;

public class Account
{

    public int Id {get; set;}

    [StringLength(20)]
    public string Name {get; set;} = string.Empty;
    
     [StringLength(20)]
    public string Username {get; set;} = string.Empty;
    public string? Email {get; set;} = string.Empty;

   
    public string? PhoneNumber {get; set;}
    public string Password {get; set;} = string.Empty;

    
}