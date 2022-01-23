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
            this.SixToEleven = cases.Count(@case => @case.Age >= 6 && @case.Age <=11);
            this.TwelveToSeventeen = cases.Count(@case => @case.Age >= 12 && @case.Age <= 17);
            this.EighteenToThirtyThree = cases.Count(@case => @case.Age >= 18 && @case.Age <= 33);
            this.ThirtyFourToFifty = cases.Count(@case => @case.Age >= 34 && @case.Age <= 50);
            this.FiftyOneToSeventy = cases.Count(@case => @case.Age >= 51 && @case.Age <= 70);
            this.MoreThanSeventyOne = cases.Count(@case => @case.Age >= 71);
        }

        public DateTime Date { get; set; }

        public long Total { get; private set; }

        public long Female { get; private set; }
        public long Male { get; private set; }

        public long ZeroToFive { get; private set; }
        public long SixToEleven { get; private set; }
        public long TwelveToSeventeen { get; private set; }
        public long EighteenToThirtyThree { get; private set; }
        public long ThirtyFourToFifty { get; private set; }
        public long FiftyOneToSeventy { get; private set; }
        public long MoreThanSeventyOne { get; private set; }

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