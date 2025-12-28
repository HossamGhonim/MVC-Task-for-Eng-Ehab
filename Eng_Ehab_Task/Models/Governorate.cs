using System.ComponentModel.DataAnnotations;

namespace Eng_Ehab_Task.Models
{
    public class Governorate
    {
        [Key] public int GovernorateID { get; set; }
        public string GovernorateName { get; set; }
    }
}
