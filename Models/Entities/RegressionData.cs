namespace Covid19.Models.Entities
{
    using System;

    public class RegressionData
    {
        public Func<double, double> Function { get; set; }
        public int DaysToSkip { get; set; }
    }
}