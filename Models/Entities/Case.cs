namespace IfCovid.Models.Entities
{
    using System;

    public class Case
    {
        public string Id { get; init; }

        public DateTime InDate { get; init; }

        public string District { get; init; }

        public string City { get; init; }

        public string Gender { get; init; }

        public int Age { get; init; }

        public bool OtherIllnesses { get; init; }

        public bool Hospitalized { get; init; }

        public bool IntensiveCare { get; init; }

        public bool Ventilated { get; init; }

        public bool Dead { get; init; }

        public DateTime OutDate { get; init; }
    }
}