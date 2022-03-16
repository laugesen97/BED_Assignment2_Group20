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
    public class ModelsController : ControllerBase
    {
        private readonly DataDb _context;
        // Laver min mapper
        MapperConfiguration mapperConfig;
        MapperConfiguration mapperConfigReverse;
        Mapper mapper;
        Mapper mapperReverse;
        public ModelsController(DataDb context)
        {
            _context = context;

            //Instansierer min mapper
            mapperConfig = new MapperConfiguration(cfg => {
                cfg.CreateMap<ModelNoJobsOrExpenses, Model>();
            });
            mapper = new Mapper(mapperConfig);
            mapperConfigReverse = new MapperConfiguration(cfg => {
                cfg.CreateMap<Model, ModelNoJobsOrExpenses>();
            });
            mapperReverse = new Mapper(mapperConfigReverse);

        }

        // GET: api/Models
        // Hente en liste med alle modeller – uden data for deres jobs eller udgifter.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Model>>> GetModels()
        {
            List<Model> models = new List<Model>();
            foreach (var item in _context.Models)
            {
                item.Expenses = null;
                item.Jobs = null;
                models.Add(item);
            }
            return models;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Model>> UpdateModel(long id, ModelNoJobsOrExpenses patchModel)
        {
            var model = await _context.Models.FindAsync(id);
            Model savedPatchModel = mapper.Map<Model>(patchModel);
            model = savedPatchModel;
            _context.Models.Update(model);
            return model;
        }

        // GET: api/Models/5
        // Hente model med den angivne ModelId inklusiv modellens jobs og udgifter.
        [HttpGet("{id}")]
        public async Task<ActionResult<Model>> GetModel(long id)
        {
            var model =  _context.Models.Where(x => x.ModelId == id).Include(d => d.Expenses).Include(d => d.Jobs).FirstOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            return model;
        }

        // PUT: api/Models/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModel(long id, ModelNoJobsOrExpenses model)
        {
            if (id != model.ModelId)
            {
                return BadRequest();
            }

            _context.Entry(mapper.Map<Model>(model)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModelExists(id))
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

        // POST: api/Models
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Opret ny model – kun grunddata – ikke jobs og udgifte
        [HttpPost]
        public async Task<ActionResult<Model>> PostModel(ModelNoJobsOrExpenses model)
        {
            Model newModel = mapper.Map<Model>(model);
            _context.Models.Add(newModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetModel", new { id = model.ModelId }, newModel);
        }

        // DELETE: api/Models/5
        // Slette en model
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModel(long id)
        {
            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ModelExists(long id)
        {
            return _context.Models.Any(e => e.ModelId == id);
        }
    }
}
