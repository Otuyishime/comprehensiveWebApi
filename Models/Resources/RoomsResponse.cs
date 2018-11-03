using System;
using testWebAPI.Models.Forms;

namespace testWebAPI.Models.Resources
{
    public class RoomsResponse : PagedCollection<Room>
    {
        public Link Openings { get; set; }
        public Form RoomsQuery { get; set; }
    }
}
