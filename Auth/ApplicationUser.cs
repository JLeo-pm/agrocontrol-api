using Microsoft.AspNetCore.Identity;

namespace SmartRancho.API.Auth;

public class ApplicationUser : IdentityUser
{
    public int RanchoId { get; set; } 
}