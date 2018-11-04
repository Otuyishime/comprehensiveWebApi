﻿using System;
using Newtonsoft.Json;
using testWebAPI.Infrastructure;
using testWebAPI.Infrastructure.Etags;

namespace testWebAPI.Models.Resources
{
    public class HotelInfo : Resource, IEtaggable
    {
        public string Title { get; set; }

        public string Tagline { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public Address Location { get; set; }

        public string GetEtag()
        {
            var serialized = JsonConvert.SerializeObject(this);
            return Md5Hash.ForString(serialized); // Quick and dirty way of getting a hash
        }
    }

    public class Address
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
