using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MultiRoleAuthentication.Areas.Identity.Data;
using MultiRoleAuthentication.DAL;
using MultiRoleAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.Controllers
{
    [Authorize(Roles = "Developer")]
    public class DeveloperController : Controller
    {
        private readonly UserManager<MultiRoleAuthUser> _userManager;
        private readonly ModelExpressionProvider _modelExpressionProvider;
        private readonly IWorkItemRepository _workItemRepository;

        public DeveloperController(UserManager<MultiRoleAuthUser> userManager,
                                ModelExpressionProvider modelExpressionProvider,
                                IWorkItemRepository workItemRepository)
        {
            _userManager = userManager;
            _modelExpressionProvider = modelExpressionProvider;
            _workItemRepository = workItemRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeveloperBoard()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(_workItemRepository.GetWorkItemsByState(WIStatus.Development).OrderByDescending(x => x.IsUrgent));
        }

        public IActionResult Development(int id, WIStatus status)
        {
            var workitem = _workItemRepository.GetWorkItemByID(id);

            if (status == WIStatus.ReturnToPO)
            {
                workitem.StateId = (int)WIStatus.ReturnToPO;

                _workItemRepository.UpdateWorkItem(workitem);
                _workItemRepository.Save();
                return RedirectToAction("DeveloperBoard", "Developer");
            }
            else
            {
                return View(workitem);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Development(WorkItem model)
        {
            if (string.IsNullOrWhiteSpace(model.DevComment))
                ModelState.AddModelError("DevComment", "The implementation is required");
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var workitem = _workItemRepository.GetWorkItemByID(model.WorkItemId);
                workitem.StateId = (int)WIStatus.Test;
                workitem.DevComment = model.DevComment;
                workitem.DevCommentCreatedBy = user.UserName;
                workitem.DevCommentCreatedOn = DateTime.UtcNow;

                _workItemRepository.UpdateWorkItem(workitem);
                _workItemRepository.Save();

                return RedirectToAction("DeveloperBoard", "Developer");
            }
            return View(model);
        }
    }
}
