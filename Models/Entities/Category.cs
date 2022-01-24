namespace Covid19.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Category
    {
        public Category()
        {
        }

        public Category(DateTime date, List<Case> cases)
        {
            this.Date = date;

            this.Total = cases.Count;

            this.Female = cases.Count(@case => @case.Gender == "Жіноча");
            this.Male = this.Total - this.Female;

            this.ZeroToFive = cases.Count(@case => @case.Age <= 5);
            this.SixToEleven = cases.Count(@case => @case.Age is >= 6 and <= 11);
            this.TwelveToSeventeen = cases.Count(@case => @case.Age is >= 12 and <= 17);
            this.EighteenToThirtyThree = cases.Count(@case => @case.Age is >= 18 and <= 33);
            this.ThirtyFourToFifty = cases.Count(@case => @case.Age is >= 34 and <= 50);
            this.FiftyOneToSeventy = cases.Count(@case => @case.Age is >= 51 and <= 70);
            this.MoreThanSeventyOne = cases.Count(@case => @case.Age >= 71);
        }

        public DateTime Date { get; private init; }

        public long Total { get; private init; }

        public long Female { get; private init; }
        public long Male { get; private init; }

        public long ZeroToFive { get; private init; }
        public long SixToEleven { get; private init; }
        public long TwelveToSeventeen { get; private init; }
        public long EighteenToThirtyThree { get; private init; }
        public long ThirtyFourToFifty { get; private init; }
        public long FiftyOneToSeventy { get; private init; }
        public long MoreThanSeventyOne { get; private init; }

        public Category GetAverageData(List<Category> categories)
        {
            categories.Add(this);

            var category = new Category
            {
                Date = this.Date,
                Total = (long)categories.Average(currentCategory => currentCategory.Total),
                Female = (long)categories.Average(currentCategory => currentCategory.Female),
                Male = (long)categories.Average(currentCategory => currentCategory.Male),
                ZeroToFive = (long)categories.Average(currentCategory => currentCategory.ZeroToFive),
                SixToEleven = (long)categories.Average(currentCategory => currentCategory.SixToEleven),
                TwelveToSeventeen = (long)categories.Average(currentCategory => currentCategory.TwelveToSeventeen),
                EighteenToThirtyThree = (long)categories.Average(currentCategory => currentCategory.EighteenToThirtyThree),
                ThirtyFourToFifty = (long)categories.Average(currentCategory => currentCategory.ThirtyFourToFifty),
                FiftyOneToSeventy = (long)categories.Average(currentCategory => currentCategory.FiftyOneToSeventy),
                MoreThanSeventyOne = (long)categories.Average(currentCategory => currentCategory.MoreThanSeventyOne),
            };

            return category;
        }
    }
}