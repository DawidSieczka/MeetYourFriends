namespace MeetYourFriends.Service.Domain.Models;

public class UserWorkSchedules
{
    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public string WorkingHours { get; set; }
}