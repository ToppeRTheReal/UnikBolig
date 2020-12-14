using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace UnikBolig.Web.Models
{
    public class EstateModels
    {
        public string Rulesetstring { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name="Navn")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Beskrivelse")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "m2")]
        public int Size { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Gadenavn")]
        public string StreetName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Husnummer")]
        public int HouseNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Antal etager")]
        public int Floor { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Postnummer")]
        public int Postal { get; set; }

        public string ImgUrl { get; set; }
    }
}
