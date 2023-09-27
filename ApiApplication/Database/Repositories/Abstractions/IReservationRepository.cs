using ApiApplication.Database.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IReservationRepository
    {
        Task<ReservationEntity> CreateReservation(ReservationEntity reservationEntity, CancellationToken cancel);
        Task<List<ReservationEntity>> GetAllReservationsAsync(CancellationToken cancel);
        Task<ReservationEntity> GetReservationByGUIDAsync(string guid, CancellationToken cancel);
    }
}
