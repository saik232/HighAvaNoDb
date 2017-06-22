using System;

namespace HighAvaNoDb.Domain
{
    public interface IAggregateRoot:IEntity
    {
        /// <summary>
        /// identification
        /// </summary>
        Guid Id { get; set; }
    }
}
