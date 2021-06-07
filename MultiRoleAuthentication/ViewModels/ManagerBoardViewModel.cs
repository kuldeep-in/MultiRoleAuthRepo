using MultiRoleAuthentication.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.ViewModels
{
    public class ManagerBoardViewModel
    {
        public bool ShowRequirementSavedMessage { get; set; }

        public int WorkItemId { get; set; }

        [Required]
        [DisplayName("Requirement number")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Urgent")]
        public bool IsUrgent { get; set; }

        public IEnumerable<WorkItem> WorkItems { get; set; }
    }
}

