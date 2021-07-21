using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ibero.Services.Utilitary.Domain.Person.Models
{
   public class PersonModel
    {

        [JsonProperty("Reference")]
        public int Reference { get; set; }

        [JsonProperty("ID")]
        public int ID { get; set; }

        [JsonProperty("IDENTIFICACION")]
        public int IDENTIFICACION { get; set; }

        [JsonProperty("Nombre")]
        public string Nombre { get; set; }

        [JsonProperty("Genero")]
        public int Genero { get; set; }

        [JsonProperty("Estado")]
        public int Estado { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

    }
}
