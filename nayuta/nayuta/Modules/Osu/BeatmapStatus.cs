﻿using System;

namespace nayuta.Modules.Osu
{
    public enum BeatmapStatus
    {
        Graveyarded = -2,
        Unranked = -1,
        Pending = 0,
        Ranked = 1,
        Approved = 2,
        Qualified = 3,
        Loved = 4
    }
}