using ApiApplication.BusinessLogic.Interfaces;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Models;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.BusinessLogic.Implementation
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
        }
        public async Task<Result> GetSeatsByAuditoriumID(int id)
        {
            Result res = new Result();
            try
            {
                var auditoriumInfo = await _auditoriumsRepository.GetAsync(id, default(CancellationToken));
                if(auditoriumInfo != null)
                {
                    res.IsSuccessful = true;
                    res.ReturnedObject = auditoriumInfo;
                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "There is no auditorium info with this ID";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message.ToString());
                res.IsSuccessful = false;
            }

            return res;
        }
    }
}
