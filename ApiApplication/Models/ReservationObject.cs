using System;

namespace ApiApplication.Models
{
    public class ReservationObject
    {
        public  string GUID { get; set; }
        public  int NumberOfSeats { get; set; }
        public  int AuditoriumID { get; set; }
        public  string MovieTitle { get; set; }
        public  int SeatNumber { get; set; }
        public  DateTime CreatedTime { get; set; }
    }
}
