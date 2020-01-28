using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sero.Keeper;
using WebTest.Models;

namespace WebTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly AppKeeper _keeper;
        private readonly UserStore _userStore;
        
        public WeatherForecastController(AppKeeper keeeper, UserStore userStore)
        {
            _keeper = _keeper;
            _userStore = userStore;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            User user = new User();
            user.Name = "Oleeeg";
            user.Nicknames.Add("Krac");
            user.Nicknames.Add("ukRa");

            int createdId = await _userStore.SaveAsync(user);

            var fetched = await _userStore.FetchAsync(createdId);

            fetched.Name = fetched.Name + " modif";

            _userStore.Update(fetched);

            //await _userStore.DeleteAsync(fetched.Id);

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
