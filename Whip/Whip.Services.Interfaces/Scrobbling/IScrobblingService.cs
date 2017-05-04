﻿using System;
using System.Threading.Tasks;
using Whip.Common.Model;

namespace Whip.Services.Interfaces
{
    public interface IScrobblingService
    {
        Task<bool> ScrobbleAsync(Track track, DateTime timePlayed);
        Task<bool> UpdateNowPlayingAsync(Track track, int duration);
    }
}
