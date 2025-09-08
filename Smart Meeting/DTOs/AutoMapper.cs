using AutoMapper;
using Microsoft.Extensions.Logging;
using Smart_Meeting.Models;

namespace Smart_Meeting.DTOs
{
    public class AutoMapper:Profile
    {

        public AutoMapper()
        {
            CreateMap<RoomFeatures, AssignFeatureDto>();
            CreateMap<Room, RoomDto>().ForMember(dest => dest.status,
                       opt => opt.MapFrom(src => src.status.ToString()));
            CreateMap<Room, CreateRoomDto>();
            CreateMap<Room, UpdateRoomDto>().ForMember(dest => dest.status,
                       opt => opt.MapFrom(src => src.status.ToString()));
            CreateMap<Employee, CreateEmployeeDto>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<Attendee, AddAttendeeDto>();
            CreateMap<Attendee, AttendeeDto>().ForMember(dest => dest.EmpFirstName, 
                                               opt => opt.MapFrom(src => src.Employee.FirstName))
                                              .ForMember(dest => dest.EmpLastName, 
                                               opt => opt.MapFrom(src => src.Employee.LastName))
                                              .ForMember(dest => dest.Job,
                                               opt => opt.MapFrom(src => src.Employee.Job))
                                              .ForMember(dest => dest.Email,
                                               opt => opt.MapFrom(src => src.Employee.Email))
                                              .ForMember(dest => dest.Avatar,
                                               opt => opt.MapFrom(src => src.Employee.Avatar));
            CreateMap<MinutesOfMeeting, MinutesOfMeetingDto>().ForMember(dest => dest.AuthorEmail,
                       opt => opt.MapFrom(src => src.Author.Email));
            CreateMap<MinutesOfMeeting, AddAuthorDto>();
            CreateMap<MinutesOfMeeting, AddSummaryDto>();

            CreateMap<Meeting, CreateMeetingDto>();
            CreateMap<Meeting, MeetingDto>().ForMember(dest => dest.status,
                       opt => opt.MapFrom(src => src.status.ToString()))
                                             .ForMember(dest => dest.CreaterFirstName,
                                                       opt => opt.MapFrom(src => src.Employee.FirstName))
                                             .ForMember(dest => dest.CreaterLastName,
                                                       opt => opt.MapFrom(src => src.Employee.LastName))
                                             .ForMember(dest => dest.RoomName,
                                                       opt => opt.MapFrom(src => src.Room.RoomName))
                                             .ForMember(dest => dest.AuthorId,
                                                       opt => opt.MapFrom(src => src.MinutesOfMeeting.AuthorId));
            CreateMap<Employee, LogInDto>();
            CreateMap<AvailableFeatures, FeatureDto>();
            CreateMap<AvailableFeatures, CreateFeatureDto>();



            CreateMap<AssignFeatureDto, RoomFeatures>();
            CreateMap<RoomDto, Room>().ForMember(dest => dest.status,
                       opt => opt.MapFrom(src => Enum.Parse<RoomStatus>(src.status)));
            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();
            CreateMap<CreateEmployeeDto ,Employee>();
            CreateMap<EmployeeDto ,Employee>();
            CreateMap<AttendeeDto ,Attendee>();
            CreateMap<AddAttendeeDto ,Attendee>();
            CreateMap<MinutesOfMeetingDto ,MinutesOfMeeting>();
            CreateMap<AddAuthorDto, MinutesOfMeeting>();
            CreateMap<AddSummaryDto, MinutesOfMeeting>();
            CreateMap<CreateMeetingDto ,Meeting>();
            CreateMap<MeetingDto, Meeting>().ForMember(dest => dest.status,
                       opt => opt.MapFrom(src => Enum.Parse<MeetingStatus>(src.status)));
            CreateMap<LogInDto, Employee>();
            CreateMap<FeatureDto, AvailableFeatures>();
            CreateMap<CreateFeatureDto, AvailableFeatures>();


        }

    }
}
