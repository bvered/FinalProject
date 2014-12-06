using System;

namespace Server.Models
{
    public interface IPersistent
    {
        Guid Id { get; set; }
    }
}