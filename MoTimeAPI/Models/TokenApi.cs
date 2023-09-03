using System;
using System.Collections.Generic;

namespace MoTimeAPI.Models;

public partial class TokenApi
{
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}
