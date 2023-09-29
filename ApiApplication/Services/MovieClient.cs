using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class MovieClient : IMovieClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public MovieClient(HttpClient client, IConfiguration configuration, IAuditoriumsRepository auditoriumsRepository)
        {
            _client = client;
            _configuration = configuration;
            _auditoriumsRepository = auditoriumsRepository;
        }

        
        

        public async Task<Result> GetMovieByID(string id)
        {
            Result res = new Result();
            Stopwatch stopwatch = new Stopwatch();
            MoviesResponseObject pickedMovie;
            
            try
            {
                var redisConnectionString = _configuration["RedisConnectionString"];
                var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                IDatabase db = redis.GetDatabase();

                string cachedData = db.StringGet("movieResponse");

                if (cachedData == null)
                {
                    _client.DefaultRequestHeaders.Clear();
                    _client.DefaultRequestHeaders.Add("X-Apikey", _configuration["APIKEY"]);

                    stopwatch.Start();
                    var movie = await _client.GetAsync(_configuration["BaseUrl"] + $"movies/{id}");
                    stopwatch.Stop();

                    var elapsed = stopwatch.Elapsed.TotalSeconds;
                    Log.Information($"The GetAllMovies endpoint took ====> {elapsed} seconds");

                    movie.EnsureSuccessStatusCode();
                    var result = await movie.Content.ReadAsStringAsync();

                    db.StringSet("movieResponse", result);

                    pickedMovie = JsonConvert.DeserializeObject<MoviesResponseObject>(result);
                   
                }
                else
                {
                    string value = db.StringGet("movieResponse");
                    pickedMovie = JsonConvert.DeserializeObject<MoviesResponseObject>(value);
                }

                if (pickedMovie.fullTitle != null)
                {
                    res.IsSuccessful = true;
                    res.ReturnedObject = pickedMovie;
                    redis.Close();

                    return res;
                }
                else
                {
                    res.IsSuccessful = false;
                    res.Message = "failed to fetch movie!";
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message.ToString());
            }

            return res;
        }
    }
}
