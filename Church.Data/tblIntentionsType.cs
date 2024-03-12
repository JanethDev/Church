using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data
{
    public class tblIntentionsType
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int IntentionTypeID { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [DTOAtribute(Name = "IntentionType")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
    }
}
