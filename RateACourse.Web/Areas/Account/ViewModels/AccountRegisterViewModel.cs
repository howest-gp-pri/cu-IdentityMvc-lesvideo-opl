using System.ComponentModel.DataAnnotations;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class AccountRegisterViewModel : AccountLoginViewModel
    {
        
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password")]
        public string RepeatPassword { get; set; }
    }
}
