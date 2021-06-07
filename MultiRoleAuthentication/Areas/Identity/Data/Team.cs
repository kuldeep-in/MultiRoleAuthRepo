
namespace MultiRoleAuthentication.Areas.Identity.Data
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Team
    {
        [Column(TypeName = "int")]
        [DisplayName("Id")]
        public int TeamId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string TeamName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CustomerId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string CustomerName { get; set; }

        [Column(TypeName = "datetime")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime EngagementDate { get; set; }
    }
}
