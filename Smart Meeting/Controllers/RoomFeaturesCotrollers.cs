using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Meeting.Data;
using Smart_Meeting.DTOs;
using Smart_Meeting.Models;

namespace Smart_Meeting.Controllers
{
    [Route("api/room/{roomId}/features")]
    [ApiController]

    public class RoomFeaturesCotrollers : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;

        public RoomFeaturesCotrollers(AppDBContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> AssignFeaturesToRoom(int roomId, AssignFeatureDto dto)
        {
            var roomExists = await _context.Rooms.FirstOrDefaultAsync(r => r.ID == roomId);
            if (roomExists == null)
                return NotFound("Room not found.");

            var validFeatureIds = await _context.AvailableFeatures
                .Where(f => dto.FeatureIDs.Contains(f.ID))
                .Select(f => f.ID)
                .ToListAsync();

            var roomFeatures = validFeatureIds.Select(fid => new RoomFeatures
            {
                RoomID = roomId,
                FeatureID = fid
            });

            _context.RoomFeatures.AddRange(roomFeatures);
            await _context.SaveChangesAsync();

            return Ok("Features assigned successfully.");
        }


        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FeatureDto>>> GetFeaturesForRoom(int roomId)
        {
            var features = await _context.RoomFeatures
                .Where(rf => rf.RoomID == roomId)
                .Include(rf => rf.AvailableFeatures)
                .Select(rf => new FeatureDto
                {
                    ID = rf.AvailableFeatures.ID,
                    Feature = rf.AvailableFeatures.Feature
                })
                .ToListAsync();

            return Ok(features);
        }

        [HttpDelete]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RemoveFeatureFromRoom(int roomId, int featureId)
        {
            var roomFeature = await _context.RoomFeatures
                .FirstOrDefaultAsync(rf => rf.RoomID == roomId && rf.FeatureID == featureId);

            if (roomFeature == null)
                return NotFound("Feature not assigned to this room.");

            _context.RoomFeatures.Remove(roomFeature);
            await _context.SaveChangesAsync();

            return Ok("Feature removed from room.");
        }




    }
}
