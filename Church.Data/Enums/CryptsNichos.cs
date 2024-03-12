using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Church.Data.Enums
{
    public enum CryptsNichos
    {
        [Display(Name = "SIMÓN EL CANANEO")]
        SimonElCananeo = 1,
        [Display(Name = "APÓSTOL MATÍAS")]
        ApostolMatias = 2,
        [Display(Name = "APÓSTOL TOMÁS")]
        ApostolTomas = 3,
        [Display(Name = "CURVA CAPILLA")]
        CurvaCapilla = 4,
        [Display(Name = "PASILLO APÓSTOLES")]
        PasilloApostoles = 5,
        [Display(Name = "Capilla cristo Resucitado")]
        CapillaCristoResucitado = 6,
        [Display(Name = "PASILLO ISAAC")]
        PasilloIsaac = 7,
        [Display(Name = "PASILLO JACOBO")]
        PasilloJacobo = 8,
        [Display(Name = "PATRIARCA ABRAHAM")]
        PatriarcaAbraham = 9,
        [Display(Name = "PASILLO ABRAHAM")]
        PasilloAbraham = 10
    }
}
