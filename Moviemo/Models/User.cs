using Microsoft.AspNetCore.Identity;

namespace Moviemo.Models
{
    public class User
    {
        public long userId {  get; set; }
        public string name {  get; set; }
        public string surname { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public UserRole userRole { get; set; }
        public ICollection<Review> reviews { get; set; }
        public ICollection<Comment> comments { get; set; }
        
    }

    public enum UserRole
    {
        Basic,
        Admin,
        Manager
    }
}
