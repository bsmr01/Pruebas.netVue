using System;
using System.Collections.Generic;
using System.Text;

namespace Ibero.Services.Utilitary.Domain.Libros.Models
{
    public class MovieModel
    {
        public int id { get; set; }
        public bool adult { get; set; }
        public string genres { get; set; }
        public string homepage { get; set; }
        public string original_language { get; set; }
        public string title { get; set; }

    }
}
