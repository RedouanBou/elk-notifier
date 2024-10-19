using System;

namespace ElkNotifier
{
    public class AlertModel
    {
        public string? Id { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}