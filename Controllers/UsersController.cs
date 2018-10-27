using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using testWebAPI.Models;
using testWebAPI.ModelViews;

namespace testWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // GET: api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var userMV = new UsersModelView();
            return userMV.RetrieveUsers().AsEnumerable();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public User Get(int id)
        {
            var userMV = new UsersModelView();
            return userMV.RetrieveUsers().FirstOrDefault(u => u.Id == id);
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
