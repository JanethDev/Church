using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Church.Data
{
    public class tblCustomers
    {

        [Display(Name = "ID")]
        public int CustomerID { get; set; }

        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerID2 { get; set; }

        [Display(Name = "Usuario")]
        public int UserID { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Apellido Paterno")]
        public string PSurname { get; set; }

        [StringLength(100)]
        [Display(Name = "Apellido Materno")]
        public string MSurname { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(250, ErrorMessage = "Correo eletrónico no debe exceder de 80 caracteres")]
        [Display(Name = "Email")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Correo eletrónico no valido")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Ciudad")]
        public int? TownID { get; set; }
        [Display(Name = "Delegación")]
        public string Delegation { get; set; }

        [StringLength(16)]
        [Display(Name = "Telefono")]
        public string Phone { get; set; }

        [StringLength(16)]
        [Display(Name = "Celular")]
        public string CelPhone { get; set; }

        [Display(Name = "Activo")]
        public bool Active { get; set; }
        [Display(Name = "Prospecto")]
        public bool Prospective { get; set; }


        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name = "Ciudad de nacimiento")]
        public string CityOfBirth { get; set; }
        [Display(Name = "Numero")]
        public string AddressNumber { get; set; }
        [Display(Name = "Numero interior")]
        public int? AdressInteriorNumber { get; set; }
        [Display(Name = "Colonia")]
        public string Neighborhood { get; set; }
        [Display(Name = "Codigo postal")]
        public int? ZipCode { get; set; }
        [Display(Name = "RFC ó CURP")]
        public string RFCCURP { get; set; }
        [Display(Name = "Razón Social")]
        public string BusinessName { get; set; }
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Estado Civil")]
        public int? CivilStatusID { get; set; }
        [Display(Name = "Ocupación")]
        public string Occupation { get; set; }
        [Display(Name = "Empresa donde labora")]
        public string Company { get; set; }
        [Display(Name = "Telefono de la empresa")]
        public string PhoneCompany { get; set; }
        [Display(Name = "Dirección de la empresa")]
        public string AddressCompany { get; set; }
        [Display(Name = "Extención")]
        public int? ExtPhoneCompany { get; set; }
        [Display(Name = "Ciudad donde se ubica la empresa")]
        public int? TownAddressCompanyID { get; set; }
        [Display(Name = "Delegación donde se ubica la empresa")]
        public string DelegationAddressCompany { get; set; }
        [Display(Name = "Ingreso")]
        public int? Income { get; set; }

        [Display(Name = "CFDI")]
        public string CFDI { get; set; }

        [Required]
        [Display(Name = "Fecha de creación")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Creado por")]
        public int CreatedBy { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Actualizado por")]
        public int UpdatedBy { get; set; }

        [Display(Name = "Fecha de eliminación")]
        public DateTime? DeletedDate { get; set; }

        [Display(Name = "Eliminado por")]
        public int? DeletedBy { get; set; }

        [NotMapped]
        public HttpPostedFileBase CFDIFile { get; set; }
        [NotMapped]
        [Display(Name = "Estado")]
        public int StateID { get; set; }
        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string UpdatedByName { get; set; }
        [NotMapped]
        public string State { get; set; }
        [NotMapped]
        public string Town { get; set; }
        [NotMapped]
        public string StateAddressCompany { get; set; }
        [NotMapped]
        public int StateAddressCompanyID { get; set; }
        [NotMapped]
        public string TownAddressCompany { get; set; }
        [NotMapped]
        public string CivilStatus { get; set; }
        [NotMapped]
        public string sDateOfBirth { get; set; }
        [NotMapped]
        public string value { get; set; }
        [NotMapped]
        public string label { get; set; }
        [NotMapped]
        public string ReferenceCustomer1 { get; set; }
        [NotMapped]
        public string ReferenceCustomerPhone1 { get; set; }
        [NotMapped]
        public string ReferenceCustomer2 { get; set; }
        [NotMapped]
        public string ReferenceCustomerPhone2 { get; set; }

    }
}
