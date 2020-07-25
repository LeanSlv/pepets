using PePets.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PePets.Repositories
{
    public interface IBreedRepository : ICRUD<BreedOfPet>, IDisposable
    {
        Task<BreedOfPet> GetFirstBreed();
        Task<IEnumerable<BreedOfPet>> GetAllBreedsOfTypeAsync(string typeName);
    }
}
