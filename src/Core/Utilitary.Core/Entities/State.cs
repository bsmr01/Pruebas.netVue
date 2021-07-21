namespace Ibero.Services.Utilitary.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("IBET_STATE")]
    public class State
    {
        public Guid Id { get; set; }

        [Column("Cod_State")]
        public string CodeState { get; set; }

        [Column("Nam_State")]
        public string NameState { get; set; }

        [Column("Cod_Country")]
        public int CodeCountry { get; set; }
    }
}
