using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Sero.Loxy.Abstractions;
using Sero.Loxy.EfCore;
using TestWeb.Logging.Events;
using TestWeb.Models;

namespace TestWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        protected readonly ILoxy _loxy;

        public ValuesController(ILoxy loxy)
        {
            _loxy = loxy;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var optBuilder = new DbContextOptionsBuilder()
                                    .EnableSensitiveDataLogging(true)
                                    .ConfigureWarnings(opts => {

                                        // Evita que se tire un log de tipo warning solo para decir que se loggean los valores de los parametros de cada query. Sin los
                                        // valores cual es el punto de loggear esta verga?
                                        opts.Ignore(CoreEventId.SensitiveDataLoggingEnabledWarning);

                                        // Sarasa de modo debug demasiado específico de EFCore como para ser util para propositos generales y es demasiado texo
                                        // Ejemplo: https://pastebin.com/q8wb7YsT
                                        opts.Ignore(CoreEventId.QueryExecutionPlanned);
                                    })
                                    .UseLoxyAsLoggerFactory(_loxy, "EFCore")
                                    .UseSqlite("Data Source=sqlite.db");

            TestDbContext db = new TestDbContext(optBuilder.Options);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            User newUser = new User { Name = "Olegsinnn" };
            db.Users.Add(newUser);
            db.SaveChanges();

            db.Users.Add(newUser);
            db.SaveChanges();

            var users = db.Users.ToList();

            try
            {
                throw new Exception("mierdita rota impredecible");
            }
            catch (Exception ex)
            {
                throw new Exception("esta es una exception nueva", ex);
            }

            _loxy.Raise(new UserCreatedEvent(newUser));

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
