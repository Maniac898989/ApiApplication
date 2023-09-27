using System;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class ShowtimeDTO
    {
        public int Id { get; set; }
        public MovieModel Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan SessionTime { get; set; }
        public int AuditoriumId { get; set; }
        public ICollection<TicketModel> Tickets { get; set; }
    }

    public class MovieModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Stars { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<ShowtimeModel> Showtimes { get; set; }
    }

    public class TicketModel
    {
        public TicketModel()
        {
            CreatedTime = DateTime.Now;
            Paid = false;
        }

        public Guid Id { get; set; }
        public int ShowtimeId { get; set; }
        public ICollection<SeatModel> Seats { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Paid { get; set; }
        public ShowtimeModel Showtime { get; set; }
    }

    public class SeatModel
    {
        public short Row { get; set; }
        public short SeatNumber { get; set; }
        public int AuditoriumId { get; set; }
        public AuditoriumModel Auditorium { get; set; }
    }

    public class AuditoriumModel
    {
        public int Id { get; set; }
        public List<ShowtimeModel> Showtimes { get; set; }
        public ICollection<SeatModel> Seats { get; set; }

    }

    public class ShowtimeModel
    {
        public int Id { get; set; }
        public MovieModel Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public TimeSpan SessionTime { get; set; }
        public int AuditoriumId { get; set; }
        public ICollection<TicketModel> Tickets { get; set; }
    }

}
