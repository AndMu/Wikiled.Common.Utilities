﻿namespace Wikiled.Common.Utilities.Resources.Config
{
    public class ResourceConfig : ILocalDownload
    {
        public string Resources { get; set; }

        public LocationConfig Location { get; set; }
    }
}
