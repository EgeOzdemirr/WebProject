﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Comment.Context;

namespace WebProject.Comment.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentStatisticsController : ControllerBase
    {
        private readonly CommentContext _commentContext;
        public CommentStatisticsController(CommentContext commentContext)
        {
            _commentContext = commentContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetCommentCount()
        {
            int value = await _commentContext.UserComments.CountAsync();
            return Ok(value);
        }
    }
}
