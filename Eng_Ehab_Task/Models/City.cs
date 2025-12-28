using System.ComponentModel.DataAnnotations;

namespace Eng_Ehab_Task.Models
{
    public class City
    {
        [Key] public int CityID { get; set; }
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "Please enter only Arabic characters.")]
        public string CityName { get; set; }
        public int GovernorateID { get; set; }
        public virtual Governorate? Governorate { get; set; }
    }
}
