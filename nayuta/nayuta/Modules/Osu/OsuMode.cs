﻿using System;

namespace nayuta.Modules.Osu
{
    [Flags]
    public enum OsuMode
    {
        Standard = 0,
        Taiko = 1,
        Catch = 2,
        Mania = 3
    }
}