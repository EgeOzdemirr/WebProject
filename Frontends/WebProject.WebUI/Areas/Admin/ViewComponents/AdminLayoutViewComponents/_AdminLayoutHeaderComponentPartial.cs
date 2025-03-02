﻿using Microsoft.AspNetCore.Mvc;
using WebProject.WebUI.Services.CommentServices;
using WebProject.WebUI.Services.Interfaces;
using WebProject.WebUI.Services.MessageServices;
using WebProject.WebUI.Services.StatisticServices.CommentStatisticServices;

namespace WebProject.WebUI.Areas.Admin.ViewComponents.AdminLayoutViewComponents
{
    public class _AdminLayoutHeaderComponentPartial:ViewComponent
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;
        private readonly ICommentStatisticService _commentStatisticService;

        public _AdminLayoutHeaderComponentPartial(IMessageService messageService, IUserService userService, ICommentStatisticService commentStatisticService)
        {
            _messageService = messageService;
            _userService = userService;
            _commentStatisticService = commentStatisticService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var appUser = await _userService.GetUserInfo();
            var appUserMessageCount = await _messageService.GetTotalMessageCountByUserId(appUser.Id);

            int totalCommentCount = await _commentStatisticService.GetCommentsCount();

            ViewBag.MessageCount = appUserMessageCount;
            ViewBag.CommentCount = totalCommentCount;

            return View();
        }
    }
}
