namespace IfCovid.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using IfCovid.Models.Entities;

    public interface IReadService
    {
        Task<List<Case>> ReadAsync();
    }
}