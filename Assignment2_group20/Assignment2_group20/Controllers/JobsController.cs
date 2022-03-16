#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment2_group20.Data;
using Assignment2_group20.Models;
using AutoMapper;

namespace Assignment2_group20.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly DataDb _context;
        // Laver min mapper
        MapperConfiguration mapperConfig;
        Mapper mapper;
        public JobsController(DataDb context)
        {
            _context = context; mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<ModelNoJobsOrExpenses, Model>();
            });
            mapper = new Mapper(mapperConfig);
        }

        // GET: api/Jobs
        // Henter alle modeller for de forskellige jobs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJobs()
        {
            var jobs = _context.Jobs.Include(x => x.Models).ToList();
            return jobs;
        }

        // GET: api/Jobs/expenses
        // Hente job med den angivne JobId. Skal inkludere listen med alle expenses for jobbet.

        [HttpGet("{jobid:long}")]
        public async Task<ActionResult<Job>> GetJobWithExpenses(long jobid)
        {
            var jobs = _context.Jobs.Where(x => x.JobId == jobid).Include(x => x.Expenses).FirstOrDefault();
            return jobs;
        }
        //GET: api/Jobs/modelsjobs/modelid
        // Hente en liste med alle jobs. Skal inkludere navn på modeller, som er sat på de enkelte
        // jobs, men ikke expenses.
        
        [HttpGet("Models{modelid:int}")]
        // Hente en liste med alle jobs for en angiven model – uden expenses.
        public async Task<ActionResult<IEnumerable<Job>>> GetJobsForModel(long modelid)
        {
            List<Job> jobs = new List<Job>();
            var contextjob = _context.Jobs.Include(x => x.Models).ToList();
            foreach (var jobse in contextjob)
            {
                foreach (var item in jobse.Models)
                {
                    if (item.ModelId == modelid)
                    {
                        jobs.Add(jobse);
                    }
                }
            }
           
            return jobs;
        }

        // GET: api/Jobs/5
        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<Job>> GetJob(long id)
        //{
        //    var job = await _context.Jobs.FindAsync(id);

        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    return job;
        //}
        [HttpPatch("{id}")]
        public async Task<ActionResult<Job>> UpdateJob(long id, DateTimeOffset StartDate, int Days, string? Location, string? Comments)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (StartDate != DateTimeOffset.MinValue)
            {
                job.StartDate = StartDate;
            }
            if (Days.ToString() != null)
            {
                job.Days = Days;
            }
            if (Location != null)
            {
                job.Location = Location;
            }
            if (Comments != null)
            {
                job.Comments = Comments;
            }

            _context.Jobs.Update(job);
            return job;
        }


        // PUT: api/Jobs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJob(long id, Job job)
        {
            if (id != job.JobId)
            {
                return BadRequest();
            }

            _context.Entry(job).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Jobs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Opret nyt job
        [HttpPost]
        public async Task<ActionResult<Job>> PostJob(Job job)
        {
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJob", new { id = job.JobId }, job);
        }
        [HttpPost("{modelid}")]
        // Tilføj model til job. Bemærk at der godt kan være flere modeller på samme job.
        public async Task<ActionResult<Job>> PostModelToJob(long jobid, ModelNoJobsOrExpenses model)
        {
            var contextjob = _context.Jobs.Where(x => x.JobId == jobid).Include(x => x.Models).Include(x => x.Expenses).FirstOrDefault();
            contextjob.Models.Add(mapper.Map<Model>(model));
            _context.Entry(contextjob).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(jobid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("AddedModel", contextjob, model);
        }

        // DELETE: api/Job
        // Slette et job
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/Jobs.Models
        // Slet model fra job
        [HttpDelete("{modelid:int}")]
        public async Task<IActionResult> DeleteModelFromJob(long jobid, long modelid)
        {
            var job = _context.Jobs.Where(x => x.JobId == jobid).Include(x => x.Models).Include(x => x.Expenses).FirstOrDefault();
            if (job == null)
            {
                return NotFound();
            }

            var model = job.Models.Where(x => x.ModelId == modelid).FirstOrDefault();
            job.Models.Remove(model);

            _context.Entry(job).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobExists(jobid))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.JobId == id);
        }
    }
}
