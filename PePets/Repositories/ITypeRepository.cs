using PePets.Models;
using System;

namespace PePets.Repositories
{
    public interface ITypeRepository : ICRUD<TypeOfPet>, IDisposable
    {
    }
}
