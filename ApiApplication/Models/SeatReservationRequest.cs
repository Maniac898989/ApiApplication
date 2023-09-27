using System;

namespace ApiApplication.Models
{
    public class SeatReservationRequest
    {
        public int NoOfReservationSeat { get; set; }
        public int movieID { get; set; }
        public string ReservationTime { get; set; }
        public DateTime ReservationDate { get; set; }
    }
}
