using System.ComponentModel.DataAnnotations;

namespace MyField.Models
{
    public class ClubManager : UserBaseModel
    {
        [Required]
        [Display(Name = "Club")]
        public int ClubId { get; set; }
        public virtual Club Club { get; set; }

        public bool IsContractEnded { get; set; }
    }
}
