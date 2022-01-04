using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUnityOfWork _unityOfWork;

        public MessagesController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetMessagesForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUserName();
            var messages = await _unityOfWork.MessageRepository.GetMessagesForUser(messageParams);
            Response.AddPaginationHeader(messages.CurrentPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUserName();
            var message = await _unityOfWork.MessageRepository.GetMessage(id);

            if (message.Recipient.UserName != username && message.Sender.UserName != username)
            {
                return Unauthorized();
            }

            if (username == message.Sender.UserName)
            {
                message.SenderDeleted = true;
            }

            if (username == message.Recipient.UserName)
            {
                message.RecipientDeleted = true;
            }

            if (message.RecipientDeleted == true && message.SenderDeleted == true)
            {
                _unityOfWork.MessageRepository.DeleteMessage(message);
            }

            if (await _unityOfWork.Complete())
            {
                return Ok();
            }

            return BadRequest("Problem deleting the message");
        }
    }
}