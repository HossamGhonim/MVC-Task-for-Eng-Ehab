using System.ComponentModel.DataAnnotations;

namespace Eng_Ehab_Task.Models
{
    public class Governorate
    {
        [Key] public int GovernorateID { get; set; }
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "Please enter only Arabic characters.")]
        public string GovernorateName { get; set; }
    }
}
