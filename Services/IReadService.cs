namespace Covid19.Services
{
    using System.Collections.Generic;

    using Covid19.Models.Entities;

    public interface IReadService
    {
        List<Case> Read();
    }
}