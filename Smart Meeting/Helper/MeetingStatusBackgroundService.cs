// MeetingStatusBackgroundService.cs
using Microsoft.EntityFrameworkCore;
using Smart_Meeting.Data;
using Smart_Meeting.Helper;

public class MeetingStatusBackgroundService : BackgroundService
{
    private readonly ILogger<MeetingStatusBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _period = TimeSpan.FromMinutes(1);

    public MeetingStatusBackgroundService(
        ILogger<MeetingStatusBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested &&
               await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                    await UpdateMeetingStatuses(dbContext);
                }

                _logger.LogInformation("Meeting statuses updated at: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating meeting statuses");
            }
        }
    }

    private async Task UpdateMeetingStatuses(AppDBContext dbContext)
    {
        var meetings = await dbContext.Meetings.Where(
            meeting => meeting.status != Smart_Meeting.Models.MeetingStatus.Completed
            && meeting.status != Smart_Meeting.Models.MeetingStatus.Canceled).ToListAsync();

        foreach (var meeting in meetings)
        {
            var calculatedStatus = GetMeetingStatus.GetStatus(
                meeting.Date, meeting.Time, meeting.EndTime);

            if (meeting.status != calculatedStatus)
            {
                meeting.status = calculatedStatus;
                dbContext.Meetings.Update(meeting);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
