
namespace MultiRoleAuthentication.DAL
{
    using MultiRoleAuthentication.Areas.Identity.Data;
    using MultiRoleAuthentication.Data;
    using MultiRoleAuthentication.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WorkItemRepository : IWorkItemRepository, IDisposable
    {
        private readonly DBContext _dbContext;

        public WorkItemRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public WorkItem GetWorkItemByID(int id)
        {
            return _dbContext.WorkItems.Find(id);
        }

        public IEnumerable<WorkItem> GetWorkItemsByState(WIStatus workItemStatus)
        {
            if (workItemStatus == WIStatus.AllStatus)
            {
                return _dbContext.WorkItems.ToList();
            }
            else
            {
                return _dbContext.WorkItems.Where(x => x.StateId == (int)workItemStatus).ToList();
            }
        }

        public IEnumerable<WorkItem> GetWorkItemsByBatchId(string batchId)
        {
            return _dbContext.WorkItems.Where(x => x.BatchId == batchId).ToList();
        }

        public void InsertWorkItem(WorkItem workItem)
        {
            _dbContext.WorkItems.Add(workItem);
        }

        public void UpdateWorkItem(WorkItem workItem)
        {
            _dbContext.WorkItems.Update(workItem);
        }

        public void UpdateWorkItemsStatusByBatchId(string batchId, WIStatus workItemStatus)
        {
            _dbContext.WorkItems.Where(x => x.BatchId == batchId).ToList().ForEach(x => x.StateId = (int)workItemStatus);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}