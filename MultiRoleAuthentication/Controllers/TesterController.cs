using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiRoleAuthentication.Areas.Identity.Data;
using MultiRoleAuthentication.DAL;
using MultiRoleAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.Controllers
{
    [Authorize(Roles = "Tester")]
    public class TesterController : Controller
    {
        private readonly UserManager<MultiRoleAuthUser> _userManager;
        private readonly IWorkItemRepository _workItemRepository;

        public TesterController(UserManager<MultiRoleAuthUser> userManager,
                                IWorkItemRepository workItemRepository)
        {
            _userManager = userManager;
            _workItemRepository = workItemRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> TesterBoard()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(_workItemRepository.GetWorkItemsByState(WIStatus.Test));
        }

        public IActionResult TestView(int id)
        {
            return View(_workItemRepository.GetWorkItemByID(id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestView(WorkItem model, string passbutton)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var workitem = _workItemRepository.GetWorkItemByID(model.WorkItemId);

                if (passbutton != null)
                {
                    workitem.StateId = (int)WIStatus.Release;
                    workitem.TestComment = string.Empty;
                    workitem.TestCommentCreatedBy = user.UserName;
                    workitem.TestCommentCreatedOn = DateTime.UtcNow;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.TestComment))
                    {
                        ModelState.AddModelError("TestComment", "Failure reason is required");
                        return View(model);
                    }
                    workitem.StateId = (int)WIStatus.Development;
                    workitem.TestComment = model.TestComment;
                    workitem.TestCommentCreatedBy = user.UserName;
                    workitem.TestCommentCreatedOn = DateTime.UtcNow;
                }

                _workItemRepository.UpdateWorkItem(workitem);
                _workItemRepository.Save();

                return RedirectToAction("TesterBoard", "Tester");
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ReturntoPO(int id)
        {
            var workitem = _workItemRepository.GetWorkItemByID(id);

            if (workitem != null)
            {
                workitem.StateId = (int)WIStatus.ReturnToPO;
                workitem.TestComment = string.Empty;

                _workItemRepository.UpdateWorkItem(workitem);
                _workItemRepository.Save();
            }
            return RedirectToAction("TesterBoard", "Tester");
        }
    }
}
