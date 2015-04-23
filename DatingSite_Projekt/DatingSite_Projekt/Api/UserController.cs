using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DatingSite_Projekt.Helpers;
using Dating_data.Repository;
using DatingSite_Projekt.Api.Models;


namespace DatingSite_Projekt.Api
{
    
    public class UserController : ApiController
    {
        [HttpGet]
        public static List<UserDto> GetAll()
        {
            var users = UserRepositories.GetUsers();

            var userDto = new List<UserDto>();

            foreach (var u in users)
            {
                var dto = new UserDto(u);
                userDto.Add(dto);
            }
            return userDto;
        }
    }
}
