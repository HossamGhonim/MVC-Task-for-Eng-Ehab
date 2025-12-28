using System.ComponentModel.DataAnnotations;

namespace Eng_Ehab_Task.Models
{
    public class Village
    {
        [Key] public int VillageID { get; set; }
        public string VillageName { get; set; }
        public int CityID { get; set; }
        public virtual City City { get; set; }
    }
}
