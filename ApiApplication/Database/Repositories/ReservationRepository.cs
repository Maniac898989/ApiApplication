using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly CinemaContext _context;

        public ReservationRepository(CinemaContext context)
        {
            _context = context;
        }


        public async Task<ReservationEntity> CreateReservation(ReservationEntity reservationEntity, CancellationToken cancel)
        {
            var reservation = await _context.Reservation.AddAsync(reservationEntity, cancel);
            await _context.SaveChangesAsync(cancel);
            return reservation.Entity;
        }

        public async Task<List<ReservationEntity>> GetAllReservationsAsync(CancellationToken cancel)
        {
            return await _context.Reservation
                .Include(x => x.Seats)
                .ToListAsync(cancel);
        }

        public async Task<ReservationEntity> GetReservationByGUIDAsync(string guid, CancellationToken cancel)
        {
            return await _context.Reservation.Include(x => x.Seats).Include(x => x.Showtime)
                .FirstOrDefaultAsync(x => x.GUID == guid, cancel);
        }
    }
}
