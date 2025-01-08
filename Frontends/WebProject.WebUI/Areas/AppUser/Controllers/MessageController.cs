using Microsoft.AspNetCore.Mvc;
using WebProject.DtoLayer.MessageDtos;
using WebProject.WebUI.Services.Interfaces;
using WebProject.WebUI.Services.MessageServices;

namespace WebProject.WebUI.Areas.AppUser.Controllers
{
    [Area("AppUser")]
    [Route("AppUser/Message")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessageController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }
        [Route("Inbox")]
        public async Task<IActionResult> Inbox()
        {
            var appUser = await _userService.GetUserInfo();
            ViewBag.appUser = appUser.Name + " " + appUser.Surname;
            var inboxMessages = await _messageService.GetInboxMessageAsync(appUser.Id);
            if (inboxMessages != null)
            {
                foreach (var item in inboxMessages)
                {
                    var allUsers = await _userService.GetAllUserInfo();
                    var sendUser = allUsers.Where(x => x.Id == item.SenderId).FirstOrDefault();
                    var senderName = sendUser.Name + " " + sendUser.Surname;
                    item.SenderName = senderName;
                }
            }
            return View(inboxMessages);
        }
        [Route("SendBox")]
        public async Task<IActionResult> SendBox()
        {
            var appUser = await _userService.GetUserInfo();
            ViewBag.appUser = appUser.Name + " " + appUser.Surname;
            var sendboxMessages = await _messageService.GetSendboxMessageAsync(appUser.Id);
            if (sendboxMessages != null)
            {
                foreach (var item in sendboxMessages)
                {
                    var allUsers = await _userService.GetAllUserInfo();
                    var receiverUser = allUsers.Where(x => x.Id == item.ReceiverId).FirstOrDefault();
                    var receiveName = receiverUser.Name + " " + receiverUser.Surname;
                    item.ReceiverName = receiveName;
                }
            }
            return View(sendboxMessages);
        }
        [Route("CreateMessage")]
        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }
        [Route("CreateMessage")]
        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var appUser = await _userService.GetUserInfo();
            createMessageDto.SenderId = appUser.Id;
            createMessageDto.ReceiverId = "3713d998-c0a0-434c-a92f-6f565c4933ac";
            createMessageDto.MessageDate = DateTime.Now.ToUniversalTime();
            createMessageDto.IsRead = false;
            await _messageService.CreateMessageAsync(createMessageDto);
            return RedirectToAction("Sendbox");
        }
        [Route("ResponseMessage/{id}")]
        [HttpGet]
        public async Task<IActionResult> ResponseMessage(int id)
        {
            var message = await _messageService.GetByIdMessageAsync(id);
            ViewBag.receiverId = message.SenderId;
            ViewBag.subject = message.Subject;
            return View();
        }
        [Route("ResponseMessage")]
        [HttpPost]
        public async Task<IActionResult> ResponseMessage(CreateMessageDto createMessageDto)
        {
            var appUser = await _userService.GetUserInfo();
            createMessageDto.SenderId = appUser.Id;
            createMessageDto.MessageDate = DateTime.Now.ToUniversalTime();
            createMessageDto.IsRead = false;
            await _messageService.CreateMessageAsync(createMessageDto);
            return RedirectToAction("Sendbox");
        }
        [Route("DeleteMessage/{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageService.GetByIdMessageAsync(id);
            await _messageService.DeleteMessageAsync(id);
            var appUser = await _userService.GetUserInfo();
            if (message.ReceiverId == appUser.Id)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Sendbox");
            }
        }
        [Route("UpdateMessage/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var message = await _messageService.GetByIdMessageAsync(id);
            return View(message);
        }
        [Route("UpdateMessage")]
        [HttpPost]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDto updateMessageDto)
        {
            var appUser = await _userService.GetUserInfo();
            updateMessageDto.MessageDate = updateMessageDto.MessageDate.ToUniversalTime();
            await _messageService.UpdateMessageAsync(updateMessageDto);

            if (updateMessageDto.ReceiverId == appUser.Id)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Sendbox");
            }
        }
        [Route("MessageDetail/{id}")]
        [HttpGet]
        public async Task<IActionResult> MessageDetail(int id)
        {
            var appUser = await _userService.GetUserInfo();
            ViewBag.appUser = appUser.Id;
            var message = await _messageService.GetByIdMessageAsync(id);
            message.IsRead = true;
            await _messageService.UpdateMessageAsync(message);
            return View(message);
        }
    }
}
