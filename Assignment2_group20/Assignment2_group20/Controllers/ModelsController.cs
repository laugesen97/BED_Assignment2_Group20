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
        // Opdater en model - kun grunddata, ikke jobs og udgifter
        public async Task<ActionResult<Model>> UpdateModel(long id, string FirstName, string LastName, string Email, string? PhoneNo, string? AddresLine1, string? AddresLine2,
            string? Zip, string? City, DateTime BirthDay, double Height, int ShoeSize, string? HairColor, string? Comments)
        {
            var model = await _context.Models.FindAsync(id);
            if (FirstName != null)
            {
                model.FirstName = FirstName;
            }
            if (LastName != null)
            {
                model.LastName = LastName;
            }
            if (Email != null)
            {
                model.Email = Email;
            }
            if (PhoneNo != null)
            {
                model.PhoneNo = PhoneNo;
            }
            if (AddresLine1 != null)
            { 
                model.AddresLine1 = AddresLine1;
            }
            if (AddresLine2 != null)
            {
                model.AddresLine2 = AddresLine2;
            }
            if (Zip != null)
            {
                model.Zip = Zip;
            }
            if (City != null)
            {
                model.City = City;
            }
            if (City != null)
            {
                model.City = City;
            }
            if (BirthDay != DateTime.MinValue)
            {
                model.BirthDay = BirthDay;
            }
            if (Height.ToString() != null)
            {
                model.Height = Height;
            }
            if (ShoeSize.ToString() != null)
            {
                model.ShoeSize = ShoeSize;
            }
            if (HairColor != null)
            {
                model.HairColor = HairColor;
            }
            if (Comments != null)
            {
                model.Comments = Comments;
            }
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
        
        // POST: api/Models
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // Opret ny model – kun grunddata – ikke jobs og udgifter
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
