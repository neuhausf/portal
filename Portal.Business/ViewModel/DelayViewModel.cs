namespace Portal.Business.ViewModel
{
    using System;

    public class DelayViewModel
    {
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int DelayInMinutes { get; set; }
    }
}