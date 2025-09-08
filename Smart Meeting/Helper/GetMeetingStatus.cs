using Smart_Meeting.Models;

namespace Smart_Meeting.Helper
{
    public class GetMeetingStatus
    {
        public static MeetingStatus GetStatus(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);

            if(currentDate < date)
            {
                return MeetingStatus.Upcoming;
            }
            else if(currentDate > date) 
            {
                return MeetingStatus.Completed;
            }
            else
            {
                if (startTime > currentTime)
                {
                    return MeetingStatus.Upcoming;
                }
                else if (endTime < currentTime)
                {
                    return MeetingStatus.Completed;
                }
                else
                {
                    return MeetingStatus.Inprogress;
                }
            }
        }
    }
}
