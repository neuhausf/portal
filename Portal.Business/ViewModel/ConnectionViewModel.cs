namespace Portal.Business.ViewModel
{
    using System.Collections.Generic;

    using Hafas;

    public class ConnectionViewModel
    {
        public ConnectionViewModel()
        {
            this.Delays = new List<DelayViewModel>();
        }
        public string GroupName { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }

        public IList<DelayViewModel> Delays { get; set; }
    }
}