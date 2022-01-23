namespace Covid19.Models.Entities
{
    using System;

    public class Case
    {
        public string Id { get; set; }

        public DateTime InDate { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }

        public bool Illnesses { get; set; }

        public bool Hospitalized { get; set; }

        public bool IntensiveCare { get; set; }

        public bool Ventilated { get; set; }

        public bool Dead { get; set; }

        public DateTime OutDate { get; set; }
    }
}