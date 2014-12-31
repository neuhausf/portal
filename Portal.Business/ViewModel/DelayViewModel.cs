namespace Portal.Business.ViewModel
{
    using System;
    using System.ServiceModel.Security;

    public class DelayViewModel
    {
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public int DelayInMinutes { get; set; }
    }
}