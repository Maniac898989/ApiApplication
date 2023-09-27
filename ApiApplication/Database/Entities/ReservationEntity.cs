using System;
using System.Collections.Generic;

namespace ApiApplication.Database.Entities
{
    public class ReservationEntity
    {
        public ReservationEntity()
        {
            CreatedTime = DateTime.Now;
        }
        
        public int Id { get; set; }
        public string GUID { get; set; }
        public int NumberOfSeats { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }
        public int AuditoriumID { get; set; }
        public ShowtimeEntity Showtime { get; set; }
        public string MovieTitle { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
