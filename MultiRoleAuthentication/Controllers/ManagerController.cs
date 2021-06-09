using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MultiRoleAuthentication.Areas.Identity.Data;
using MultiRoleAuthentication.DAL;
using MultiRoleAuthentication.Models;
using MultiRoleAuthentication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiRoleAuthentication.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        private readonly UserManager<MultiRoleAuthUser> _userManager;
        private readonly ModelExpressionProvider _modelExpressionProvider;
        private readonly IWorkItemRepository _workItemRepository;

        public ManagerController(UserManager<MultiRoleAuthUser> userManager,
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

        public async Task<IActionResult> ManagerBoard(int id, bool showRequirementSavedMessage = false)
        {
            var user = await _userManager.GetUserAsync(User);
            if (id > 0)
            {
                WorkItem workitem = _workItemRepository.GetWorkItemByID(id);
                ManagerBoardViewModel viewmodel = new ManagerBoardViewModel
                {
                    WorkItemId = workitem.WorkItemId,
                    Title = workitem.Title,
                    Description = workitem.PoComment,
                    IsUrgent = workitem.IsUrgent,
                    WorkItems = _workItemRepository.GetWorkItemsByState(WIStatus.ReturnToPO).Where(x => x.WorkItemId != id),
                    ShowRequirementSavedMessage = showRequirementSavedMessage
                };
                return View(viewmodel);
            }
            else
            {
                ManagerBoardViewModel viewmodel = new ManagerBoardViewModel
                {
                    WorkItems = _workItemRepository.GetWorkItemsByState(WIStatus.ReturnToPO),
                    ShowRequirementSavedMessage = showRequirementSavedMessage
                };
                return View(viewmodel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManagerBoard(ManagerBoardViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (viewmodel.WorkItemId == 0)
                {
                    _workItemRepository.InsertWorkItem(
                        new WorkItem
                        {
                            Title = viewmodel.Title,
                            WIDescription = "",
                            IsUrgent = viewmodel.IsUrgent,
                            StateId = (int)WIStatus.Development,
                            TeamId = 0,
                            PoComment = viewmodel.Description,
                            CreatedBy = user.UserName,
                            CreatedOn = DateTime.UtcNow,
                            PoCommentCreatedBy = user.UserName,
                            PoCommentCreatedOn = DateTime.UtcNow

                        });
                }
                else
                {
                    WorkItem workitem = _workItemRepository.GetWorkItemByID(viewmodel.WorkItemId);
                    workitem.PoComment = viewmodel.Description;
                    workitem.IsUrgent = viewmodel.IsUrgent;
                    workitem.StateId = (int)WIStatus.Development;
                }
                _workItemRepository.Save();
                return RedirectToAction("ManagerBoard", new { id = 0, showRequirementSavedMessage = true });
            }
            return View(viewmodel);
        }
    }
}
