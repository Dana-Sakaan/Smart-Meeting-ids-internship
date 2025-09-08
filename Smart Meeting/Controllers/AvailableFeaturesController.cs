using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Meeting.Data;
using Smart_Meeting.DTOs;
using Smart_Meeting.Models;

namespace Smart_Meeting.Controllers
{
    [Route("api/features")]
    [ApiController]
    public class AvailableFeaturesController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public AvailableFeaturesController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult> AddFeature(CreateFeatureDto feature)
        {
            var FeatureExist = await _context.AvailableFeatures.FirstOrDefaultAsync(f => f.Feature == feature.Feature);
            if (FeatureExist != null) return Conflict("Feature already exists");
       
            var newFeature = _mapper.Map<AvailableFeatures>(feature);
            _context.AvailableFeatures.Add(newFeature);
            await _context.SaveChangesAsync();
            return Ok("feature added");
        }

        [HttpGet] //gets all features 
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<AvailableFeatures>>> GetFeatures()
        {
            var features = await _context.AvailableFeatures.ToListAsync();
            var dtoResult = _mapper.Map<List<FeatureDto>>(features);
            return Ok(dtoResult);
        }

    }
}
