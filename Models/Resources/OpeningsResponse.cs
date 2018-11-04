using System;
using testWebAPI.Models.Forms;

namespace testWebAPI.Models.Resources
{
    public class OpeningsResponse : PagedCollection<Opening>
    {
        public Form OpeningsQuery { get; set; }
    }
}
