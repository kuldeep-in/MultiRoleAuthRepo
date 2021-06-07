
namespace MultiRoleAuthentication.DAL
{
    using MultiRoleAuthentication.Areas.Identity.Data;
    using MultiRoleAuthentication.Models;
    using System;
    using System.Collections.Generic;

    public interface IWorkItemRepository : IDisposable
    {
        WorkItem GetWorkItemByID(int id);
        IEnumerable<WorkItem> GetWorkItemsByState(WIStatus workItemStatus);
        IEnumerable<WorkItem> GetWorkItemsByBatchId(string batchId);
        void InsertWorkItem(WorkItem workItem);
        void UpdateWorkItem(WorkItem workItem);
        void UpdateWorkItemsStatusByBatchId(string batchId, WIStatus workItemStatus);
        void Save();
    }
}
