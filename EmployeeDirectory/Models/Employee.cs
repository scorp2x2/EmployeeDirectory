using System.ComponentModel.DataAnnotations;

namespace EmployeeDirectory.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Название отдела")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Номер телефона")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Display(Name = "Аватар")]
        public byte[] Avatar { get; set; }
    }
}
