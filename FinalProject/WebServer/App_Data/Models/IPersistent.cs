using System;

namespace WebServer.App_Data.Models
{
    public interface IPersistent
    {
        Guid Id { get; set; }
    }
}