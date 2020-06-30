namespace BlogSitesi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorum")]
    public partial class Yorum
    {
        public int yorumId { get; set; }

        [StringLength(500)]
        public string icerik { get; set; }

        public int? uyeId { get; set; }

        public int? makaleId { get; set; }

        public DateTime? tarih { get; set; }

        public virtual Makale Makale { get; set; }

        public virtual Uye Uye { get; set; }
    }
}
