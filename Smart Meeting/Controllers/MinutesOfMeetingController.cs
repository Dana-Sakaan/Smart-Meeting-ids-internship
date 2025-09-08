using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Meeting.Data;
using Smart_Meeting.DTOs;
using Smart_Meeting.Models;

namespace Smart_Meeting.Controllers
{
    [Route("api/{meetingID}/MoM")]
    [ApiController]
    public class MinutesOfMeetingController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        public MinutesOfMeetingController(AppDBContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MinutesOfMeetingDto>>> GetMOM(int meetingID)
        {
            var MeetingExist = await _context.Meetings.FindAsync(meetingID);
            if (MeetingExist == null) return NotFound();

            var MinsMeeting = await _context.MinutesOfMeetings.Include(mm => mm.Author)
             .FirstOrDefaultAsync(M => M.MeetingID == meetingID);
            

            var dtoResult = _mapper.Map<MinutesOfMeetingDto>(MinsMeeting);

            return Ok(dtoResult);
        }

        [HttpPost("addauthor")]
        [Authorize(Policy = "OwnerOnly")]
        public async Task<ActionResult> AddAuthor(int meetingID,AddAuthorDto author)
        {
            var claimValue = User.FindFirst("employee_id")?.Value;
            if (claimValue == null) return BadRequest();
            

            var meeting = await _context.Meetings.FirstOrDefaultAsync(meeting=> meeting.ID == meetingID);

            if (meeting.EmployeeID != claimValue) return Forbid("You are not the creator of this meeting.");

            var existingMinutes = await _context.MinutesOfMeetings
       .FirstOrDefaultAsync(mom => mom.MeetingID == meetingID);

            if (existingMinutes != null)
            {
                // Update the existing minutes entry with the new author
                _mapper.Map(author, existingMinutes); // This maps properties from the DTO to the existing entity
                _context.MinutesOfMeetings.Update(existingMinutes);
            }
            else
            {
                // Create a new minutes entry
                var newMinutes = _mapper.Map<MinutesOfMeeting>(author);
                // Ensure the foreign key is explicitly set
                newMinutes.MeetingID = meetingID;
                _context.MinutesOfMeetings.Add(newMinutes);
            }
            await _context.SaveChangesAsync();
            return Ok("author added");
        }

        [HttpPost("addsummary")]
        [Authorize]
        public async Task<ActionResult> AddMinutesSummary(int meetingID, AddSummaryDto MinSummary)
        {
            var claimValue = User.FindFirst("employee_id")?.Value;
            if (claimValue == null) return BadRequest();

            var minutesMeeting = await _context.MinutesOfMeetings.FirstOrDefaultAsync(mom => mom.MeetingID == meetingID);

            if (minutesMeeting == null) { return NotFound(); }

            if (minutesMeeting.AuthorId != claimValue) return Forbid("You cant add summary to this meeting.");

            _mapper.Map(MinSummary, minutesMeeting);
            await _context.SaveChangesAsync();
            return Ok("summary added");
        }

    }
}
