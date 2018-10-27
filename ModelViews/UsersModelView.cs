using System;
using System.IO;
using System.Collections.Generic;
using testWebAPI.Models;

namespace testWebAPI.ModelViews
{
    public class UsersModelView
    {
        public UsersModelView()
        {
        }

        public List<User> RetrieveUsers(){

            return new List<User>(){ 
                new User(1, "First User", 20), 
                new User(2, "Second User", 18)
            };
        }
    }
}
