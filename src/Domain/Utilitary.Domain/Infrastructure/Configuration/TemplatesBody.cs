namespace Ibero.Services.Utilitary.Domain.Infrastructure.Configuration
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TemplatesBody
    {
        [Column("Id_Template")]
        public int IdTemplate { get; set; }
        [Column("Cod_Template")]
        public string CodTemplate { get; set; }
        [Column("Type_Template")]
        public string TypeTemplate { get; set; }
        [Column("Name_Template")]
        public string NameTemplate { get; set; }
        [Column("Body_Template")]
        public string BodyTemplate { get; set; }
        [Column("Sta_Status")]
        public Boolean StaStatus { get; set; }

    }
}
