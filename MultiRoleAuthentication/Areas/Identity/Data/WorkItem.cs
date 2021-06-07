
namespace MultiRoleAuthentication.Areas.Identity.Data
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class WorkItem
    {
        [Column(TypeName = "int")]
        [DisplayName("Id")]
        public int WorkItemId { get; set; }

        [DisplayName("Requirement number")]
        [Column(TypeName = "nvarchar(100)")]
        public string Title { get; set; }

        [DisplayName("Requirement description")]
        public string PoComment { get; set; }

        [Column(TypeName = "bit")]
        public bool IsUrgent { get; set; }

        public int TeamId { get; set; }

        public int StateId { get; set; }

        public string CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

#nullable enable
        [Column(TypeName = "nvarchar(50)")]
        public string? BatchId { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string? WIDescription { get; set; }

        public string? PoCommentCreatedBy { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM hh:mm tt}")]
        public DateTime? PoCommentCreatedOn { get; set; }

        [DisplayName("Implementation")]
        public string? DevComment { get; set; }

        public string? DevCommentCreatedBy { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM hh:mm tt}")]
        public DateTime? DevCommentCreatedOn { get; set; }

        [DisplayName("Tester comment")]
        public string? TestComment { get; set; }

        public string? TestCommentCreatedBy { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd-MMM hh:mm tt}")]
        public DateTime? TestCommentCreatedOn { get; set; }
    }
}
