using System.ComponentModel.DataAnnotations;

namespace Eng_Ehab_Task.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string UserRole { get; set; } 
    }
}