namespace Ibero.Services.Utilitary.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("IBET_DOCUMENTS_TYPES")]
    public class DocumentType
    {        

        [Column("Id_DocumentType")]
        public int IdDocumentType { get; set; }

        [Column("Nam_DocumentType")]
        public string NameDocumentType { get; set; }

        [Column("Cod_DocumentType")]
        public string CodeDocumetType { get; set; }

        [Column("Hom_Banner")]
        public string Hom_Banner { get; set; }

        [Column("Hom_Iceberg")]
        public string HomIceberg { get; set; }

        [Column("Hom_Zoho")]
        public string HomZoho { get; set; }

        [Column("Sta_Active")]
        public Boolean StaActive { get; set; }
    }
}
