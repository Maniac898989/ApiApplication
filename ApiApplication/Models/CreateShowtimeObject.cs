using System;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class CreateShowtimeObject
    {
        public string Id { get; set; }
        public string Movie { get; set; }
        public int AuditoriumId { get; set; }
        public string ShowTime { get; set; }
        public string ShowDate { get; set; }
    }
}
