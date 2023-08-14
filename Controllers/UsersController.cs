using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
using UserService.Data;
using UserService.Dtos;
using UserService.Models;
using UserService.SyncDataServices.Http;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _reporistory;
        private readonly IMapper _mapper;
        private readonly IForumDataClient _forumDataClient;
        private readonly IMessageBusClient _messageBusClient;

        public UsersController(IUserRepo repository, IMapper mapper, IForumDataClient forumDataClient, IMessageBusClient messageBusClient)
        {
            _reporistory = repository;
            _mapper = mapper;
            _forumDataClient = forumDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetUsers()
        {
            Console.WriteLine("--> Getting Users");
            var userItems = _reporistory.GetAllUsers();
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userItems));
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserReadDto> GetUserById(int id)
        {
            Console.WriteLine("--> Getting User");
            var userItem = _reporistory.GetUserById(id);
            if (userItem != null)
            {
                return Ok(_mapper.Map<UserReadDto>(userItem));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<UserReadDto>> CreateUser(UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);
            _reporistory.CreateUser(userModel);
            _reporistory.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);
            // Send Sync Message
            try
            {
                await _forumDataClient.SendUserToForum(userReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Synchro Error: {ex.Message}");
            }

            // Send Async Message
            try
            {
                var userPublishedDto = _mapper.Map<UserPublishedDto>(userReadDto);
                userPublishedDto.Event = "User_Published";
                _messageBusClient.PublishNewUser(userPublishedDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Asynchro Error: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetUserById), new { Id = userReadDto.Id }, userReadDto);
        }

    }
}