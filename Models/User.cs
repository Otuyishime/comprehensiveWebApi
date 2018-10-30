using System;
namespace testWebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public User(int id, String name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
    }
}
