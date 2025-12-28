using System.ComponentModel.DataAnnotations;

namespace Eng_Ehab_Task.Models
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Display(Name = "الاسم الأول")]
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "يجب أن يكون الاسم الأول بين 2 و 50 حرفًا")]
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "يجب أن يكون الاسم باللغة العربية")]
        public string FirstName { get; set; }

        [Display(Name = "الاسم الأخير")]
        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "يجب أن يكون الاسم الأخير بين 2 و 50 حرفًا")]
        [RegularExpression(@"^[\u0600-\u06FF\s]+$", ErrorMessage = "يجب أن يكون الاسم باللغة العربية")]
        public string LastName { get; set; }

        [Display(Name = "الرقم القومي")]
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "يجب أن يكون الرقم القومي 14 رقمًا")]
        [RegularExpression(@"^[23]\d{13}$", ErrorMessage = "يجب أن يبدأ الرقم القومي ب 2 أو 3 ويجب أن يكون الرقم القومي 14 رقمًا")]
        public string SSN { get; set; }

        [Display(Name = "النوع")]
        [Required(ErrorMessage = "النوع مطلوب")]
        public string Gender { get; set; }

        [Display(Name = "الموقف من التجنيد")]
        public string? MilitaryStatus { get; set; }

        [Display(Name = "الراتب")]
        [Required(ErrorMessage = "الراتب مطلوب")]
        [Range(15000, 30000, ErrorMessage = "يجب أن يكون الراتب بين 15000 و 30000")]
        public decimal Salary { get; set; }

        [Display(Name = "الحالة الاجتماعية")]
        [Required(ErrorMessage = "الحالة الاجتماعية مطلوبة")]
        public string MaritalStatus { get; set; }

        [Display(Name = "المحافظة")]
        [Required(ErrorMessage = "المحافظة مطلوبة")]
        public int GovernorateID { get; set; }

        [Display(Name = "المدينة")]
        public int? CityID { get; set; }

        [Display(Name = "القرية")]
        public int? VillageID { get; set; }
    }
}