﻿namespace Server.Models;

public class Settings
{
    public string BearerKey { get; set; }
    public string PepperKey { get; set; }
    
    public int AccessTokenExpiryHours { get; set; } //Done in Hours
    public int RefreshTokenExpiryDays { get; set; } //Done In Days
}