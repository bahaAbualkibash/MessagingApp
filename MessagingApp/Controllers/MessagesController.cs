using AutoMapper;
using MessagingApp.Extentions;
using MessagingApp.Models;
using MessagingApp.Models.DT0s;
using MessagingApp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessagingApp.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private IUserRepository _userRepo;
        private IMessageRepository _messageRepo;
        private IMapper _mapper;

        public MessagesController(IUserRepository userRepository,IMessageRepository messageRepository,IMapper mapper)
        {
            _userRepo = userRepository;
            _messageRepo = messageRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<MeesageDto>> CreateMessage(CreateMessageDto createMessageDto)
        {
            var username = User.GetUsername();

            if (username == createMessageDto.RecipientUsername.ToLower())
                return BadRequest("You cannot send message to yourself");

            var sender = await _userRepo.GetUserByUsernameAsync(username); ;
            var recipient = await _userRepo.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDto.Content

            };
            _messageRepo.AddMessage(message);
            
            if(await _messageRepo.SaveAllAsync()) return Ok(_mapper.Map<MeesageDto>(message));
            return BadRequest("Failed To Send Message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeesageDto>>> GetMessagesForUser([FromQuery] 
        MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();
            var messages = await _messageRepo.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);
            return messages;

        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MeesageDto>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();

            return Ok(await _messageRepo.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await _messageRepo.GetMessage(id);

            if (message.Sender.UserName != username && message.Recipient.UserName != username) return Unauthorized();

            if(message.Sender.UserName == username) message.SenderDeleted = true;

            if (message.Recipient.UserName == username) message.RecipientDeleted = true;

            if (message.SenderDeleted && message.RecipientDeleted)
                _messageRepo.DeleteMessage(message);

            if (await _messageRepo.SaveAllAsync()) return Ok();
            return BadRequest("Problem deleteing the message");
        }
    }
}
