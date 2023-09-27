using System;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class Showtime
    {
        public DateTime ShowDate { get; set; }
        public string ShowTime { get; set; }
        public string MovieID { get; set; }
        public int AuditoriumID { get; set; }
    }
}
