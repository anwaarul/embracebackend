using System.ComponentModel.DataAnnotations;

namespace Embrace.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}