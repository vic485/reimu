﻿namespace Reimu.Common.Data
{
    /// <summary>
    /// Base parts for information stored in the database
    /// </summary>
    public abstract class DatabaseItem
    {
        /// <summary>
        /// Unique Id of the data
        /// </summary>
        public string Id { get; set; }
    }
}
