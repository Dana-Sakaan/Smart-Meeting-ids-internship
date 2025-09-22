using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smart_Meeting.Data;
using Smart_Meeting.DTOs;
using Smart_Meeting.Models;
using System;
using System.Security.Claims;

namespace Smart_Meeting.Controllers
{
    [Route("api/meeting")]
    [ApiController]
    public class MeetingControllers : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IMapper _mapper;
        public MeetingControllers(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MeetingDto>>> GetMeetings()
        {
                // Return all meetings (except completed) 
                var meetings = await _context.Meetings
                                 .Where(meeting => meeting.status != MeetingStatus.Completed)
                                 .Include(m => m.Room)
                                 .Include(m => m.Employee)
                                 .ToListAsync();
                var dtoResult = _mapper.Map<List<MeetingDto>>(meetings);
                return Ok(dtoResult);
            
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MeetingDto>> GetMeeting(int id)
        {
            var meeting = await _context.Meetings
                                .Include(meeting => meeting.Room)
                                .Include(meeting => meeting.Employee)
                                .FirstOrDefaultAsync(meeting => meeting.ID == id);

            if (meeting == null)
                return NotFound();

            var dtoResult = _mapper.Map<MeetingDto>(meeting);
            return Ok(dtoResult);

        }

        [HttpGet("employeemeetings/{id}")] //employee meetingsat a specific date
        [Authorize(Policy = "EmployeeOrAdmin")] 
        public async Task<ActionResult<IEnumerable<MeetingDto>>> GetEmpMeetings(string id, DateOnly date)
        {
            var EmpMeetings = await _context.Meetings
                              .Where(meeting => (meeting.EmployeeID == id || 
                                    meeting.Attendees.Any(a => a.EmployeeID == id))  
                                     && meeting.Date == date)
                              .Include(meeting => meeting.Room)
                              .Include(meeting => meeting.Employee)
                              .Include(meeting=> meeting.Attendees)
                              .ToListAsync();
            var dtoResult = _mapper.Map<List<MeetingDto>>(EmpMeetings);
            return dtoResult;

        } 

        [HttpGet("employeeallmeetings/{id}")] //all employee meetings
        [Authorize(Policy = "EmployeeOrAdmin")] 
        public async Task<ActionResult<IEnumerable<MeetingDto>>> GetAllEmpMeetings(string id)
        {
            var EmpMeetings = await _context.Meetings
                              .Where(meeting => (meeting.EmployeeID == id || 
                                    meeting.Attendees.Any(a => a.EmployeeID == id)) && meeting.status != MeetingStatus.Completed)  
                              .Include(meeting => meeting.Room)
                              .Include(meeting => meeting.Employee)
                              .Include(meeting=> meeting.Attendees)
                              .ToListAsync();
            var dtoResult = _mapper.Map<List<MeetingDto>>(EmpMeetings);
            return dtoResult;

        }

        [HttpGet("availablerooms")]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<IActionResult> GetAvailableRooms(DateOnly date, TimeOnly time, int duration)
        {
            var endTime = time.AddMinutes(duration);

            var availableRooms = await _context.Rooms
                    .Where(room => room.status != RoomStatus.UnderMaintenance &&
                    !   _context.Meetings
                        .Any(m => m.status != MeetingStatus.Canceled &&
                                  m.Date == date &&
                                  m.RoomID == room.ID &&
                                  time < m.Time.AddMinutes(m.Duration) &&
                                  endTime > m.Time))
                    .ToListAsync();

            var dtoResult = _mapper.Map<List<RoomDto>>(availableRooms);
            return Ok(dtoResult);
        }


        [HttpGet("availableemployees")]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAvailableEmployees(DateOnly date, TimeOnly time, int duration)
        {
            var endTime = time.AddMinutes(duration);

            // Get current user ID as string
            var currentUserId = User.FindFirst("employee_id")?.Value;

            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("Employee ID claim not found");
            }

            var availableEmployees = await _context.Employees
    .Where(employee => employee.Id != currentUserId &&
        !_context.Meetings
        .Any(m => m.status != MeetingStatus.Canceled && // Changed from Completed to Canceled
                 m.Date == date &&
                 (m.EmployeeID == employee.Id || // Meeting creator
                  m.Attendees.Any(a => a.EmployeeID == employee.Id)) && // Attendee
                 time < m.EndTime &&
                 endTime > m.Time))
    .ToListAsync();

            var dtoResult = _mapper.Map<List<EmployeeDto>>(availableEmployees);
            return Ok(dtoResult);
        }

        [HttpPost]
        [Authorize(Policy = "EmployeeOrAdmin")]
        public async Task<ActionResult<MeetingDto>> CreateMeeing(CreateMeetingDto meeting)
        {
            var MeetingExist = await _context.Meetings
                  .FirstOrDefaultAsync(m => m.Date == meeting.Date && m.Time == meeting.Time && m.RoomID == meeting.RoomID);
            if (MeetingExist != null) return Conflict("Meeting at this date and room exist");

            var newMeeting = _mapper.Map<Meeting>(meeting);
            newMeeting.EndTime = newMeeting.Time.AddMinutes(newMeeting.Duration);
            _context.Meetings.Add(newMeeting);
            await _context.SaveChangesAsync();
            var dtoResult = _mapper.Map<MeetingDto>(newMeeting);
            return Ok(dtoResult);
        }

        [HttpGet("filter")] //for all company meetings
        [Authorize]
        public async Task<ActionResult<IEnumerable<MeetingDto>>> FilteredMeetings(
            [FromQuery] string searchTerm = null,
            [FromQuery] string status = null,
            [FromQuery] string dateFilter = null) // "today", "past", "future"
        {
            IQueryable<Meeting> query = _context.Meetings
                .Include(m => m.Room)
                .Include(m => m.Employee); //to set multiple queries

            // Apply status filter (default to exclude Completed if no status specified)
            if (!string.IsNullOrEmpty(status))
            {var statusEnum = Enum.Parse<MeetingStatus>(status, true);
                query = query.Where(meeting => meeting.status == statusEnum);
          
            }
            // Apply date filter
            var today = DateOnly.FromDateTime(DateTime.Now);
            if (!string.IsNullOrEmpty(dateFilter))
            {
                switch (dateFilter.ToLower())
                {
                    case "today":
                        query = query.Where(meeting => meeting.Date == today);
                        break;
                    case "past":
                        query = query.Where(meeting => meeting.Date < today);
                        break;
                    case "future":
                        query = query.Where(meeting => meeting.Date > today);
                        break;
                }
            }

            // Apply search term filter (assuming you want to search in meeting title/description)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(meeting =>
                    meeting.Title.ToLower().Contains(searchTerm) ||
                    meeting.Description.ToLower().Contains(searchTerm));
            }

            var meetings = await query.ToListAsync();
            var dtoResult = _mapper.Map<List<MeetingDto>>(meetings);

            return Ok(dtoResult);
        }


        [HttpGet("filterempmeetings")] //for each employee meetings
        [Authorize("OwnerOnly")]
        public async Task<ActionResult<IEnumerable<MeetingDto>>> FilteredEmpMeetings(
            [FromQuery] string searchTerm = null,
            [FromQuery] string status = null,
            [FromQuery] string dateFilter = null) // "today", "past", "future"
        {
            var claimValue = User.FindFirst("employee_id")?.Value;
            if (claimValue == null) return Forbid();

            IQueryable<Meeting> query = _context.Meetings
                .Where(m=> m.EmployeeID == claimValue || m.Attendees.Any(attendee=> attendee.EmployeeID == claimValue))
                .Include(m => m.Room)
                .Include(m => m.Employee)
                .Include(m=> m.Attendees); //to set multiple queries

            // Apply status filter (default to exclude Completed if no status specified)
            if (!string.IsNullOrEmpty(status))
            {var statusEnum = Enum.Parse<MeetingStatus>(status, true);
                query = query.Where(meeting => meeting.status == statusEnum);
          
            }
            // Apply date filter
            var today = DateOnly.FromDateTime(DateTime.Now);
            if (!string.IsNullOrEmpty(dateFilter))
            {
                switch (dateFilter.ToLower())
                {
                    case "today":
                        query = query.Where(meeting => meeting.Date == today);
                        break;
                    case "past":
                        query = query.Where(meeting => meeting.Date < today);
                        break;
                    case "future":
                        query = query.Where(meeting => meeting.Date > today);
                        break;
                }
            }

            // Apply search term filter (assuming you want to search in meeting title/description)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(meeting =>
                    meeting.Title.ToLower().Contains(searchTerm) ||
                    meeting.Description.ToLower().Contains(searchTerm));
            }

            var meetings = await query.ToListAsync();
            var dtoResult = _mapper.Map<List<MeetingDto>>(meetings);

            return Ok(dtoResult);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "OwnerOnly")]
        public async Task<IActionResult> UpdateMeeting(int id, CreateMeetingDto meeting)
        {
            var ExistMeeting = await _context.Meetings.FindAsync(id);
            if (ExistMeeting == null)
                return NotFound();

            _mapper.Map(meeting, ExistMeeting);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("cancelmeeting/{id}")]
        [Authorize(Policy="OwnerOrAdmin")]
        public async Task<ActionResult> CancelMeeting(int id)
        {
            var claimValue = User.FindFirst("employee_id")?.Value;
            if (claimValue == null) return Forbid("UnAuthorized");

            var meeting = await _context.Meetings.FindAsync(id);
            if(meeting == null) return NotFound("Meeting not found");

            var employee = await _context.Employees.FindAsync(claimValue);
            if(employee == null) return NotFound("Employee not found");

            if(meeting.EmployeeID == claimValue || employee.Role == "Admin")
            {
                meeting.status = MeetingStatus.Canceled;
                await _context.SaveChangesAsync();
                return Ok("Meeting canceled");
            }
            else
            {
                return Forbid("UnAuthorized");
            }
        }
    }
}

