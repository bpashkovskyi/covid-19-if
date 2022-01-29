namespace Covid19.ViewModels
{
    using System.Collections.Generic;

    public class ChartsViewModel
    {
        public List<Chart> Charts { get; set; }

        public ViewSettings Settings { get; set; }
    }
}