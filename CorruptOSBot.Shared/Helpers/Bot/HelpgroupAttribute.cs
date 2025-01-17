﻿using System;

namespace CorruptOSBot.Shared.Helpers.Bot
{
    public class HelpgroupAttribute : Attribute
    {
        private HelpGroup _helpGroup { get; set; }
        public HelpGroup HelpGroup
        { get { return _helpGroup; } }

        public HelpgroupAttribute(HelpGroup helpGroup)
        {
            _helpGroup = helpGroup;
        }
    }

    public enum HelpGroup
    {
        Undefined,
        Everybody,
        Member,
        Staff,
        Admin
    }
}