using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace L1_TalentHub.Models
{
    public class Registros: IComparable
    {
        [Required(ErrorMessage = "Rellene el campo del Nombre")]
        [Display(Name = "Nombre")]
        [JsonProperty(PropertyName = "name")]//Hacer coincidir los nombres del Json con las de la clase Registro
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Rellene el campo del DPI")]
        [Display(Name = "DPI")]
        [JsonProperty(PropertyName = "dpi")]//Hacer coincidir los nombres del Json con las de la clase Registro
        public string Dpi { get; set; }

        [Required(ErrorMessage = "Rellene el campo de la Fecha De Nacimiento")]
        [Display(Name = "Fecha De Nacimiento")]
        [JsonProperty(PropertyName = "datebirth")]//Hacer coincidir los nombres del Json con las de la clase Registro
        public string Nacimiento { get; set; }

        [Required(ErrorMessage = "Rellene el campo de la Dirección")]
        [Display(Name = "Dirección")]
        [JsonProperty(PropertyName = "address")]//Hacer coincidir los nombres del Json con las de la clase Registro
        public string Direccion { get; set; }

        [JsonIgnore]
        public Comparison<Registros> InsertarPorDPI = delegate (Registros registro1, Registros registro2)
        {
            return registro1.Dpi.CompareTo(registro2.Dpi);
        };
        
        [JsonIgnore]
        public Comparison<Registros> InsertarPorNombre = delegate (Registros registro1, Registros registro2)
        {
            return registro1.Nombre.CompareTo(registro2.Nombre);
        };


        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

    }
}