﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectB.Services
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
        DateTime Today { get; }
    }
}
