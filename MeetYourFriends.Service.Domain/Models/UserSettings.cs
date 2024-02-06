using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetYourFriends.Service.Domain.Models;

public class UserSettings
{
    public int UserId { get; set; }
    public string? RegularWorkTimeA { get; set; }
    public string? RegularWorkTimeB { get; set; }
    public string? RegularWorkTimeC { get; set; }
}