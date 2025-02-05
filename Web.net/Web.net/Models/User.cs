﻿using System;
using System.Collections.Generic;

namespace API.Net.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } 

    public string? Email { get; set; }

    public string Password { get; set; } 

    public int? PhoneNumber { get; set; }

    public int RoleId { get; set; }

    public DateTime CreateAt { get; set; }


}
