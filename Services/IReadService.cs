namespace Covid19.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Covid19.Models.Entities;

    public interface IReadService
    {
        Task<List<Case>> ReadAsync();
    }
}