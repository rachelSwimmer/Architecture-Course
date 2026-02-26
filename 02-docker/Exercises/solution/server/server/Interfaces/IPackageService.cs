using server.DTOs;
using server.Models;

namespace server.Interfaces
{
    public interface IPackageService
    {
        Task<IEnumerable<PackageResponseDtos>> GetAll();
        Task<PackageResponseDtos> GetById(int id);
        Task<PackageResponseDtos> AddPackage(PackageCreateDtos package);
        Task<PackageResponseDtos> UpdatePackage(int id,PackageUpdateDtos package);
        Task<bool> DeletePackage(int id);
        Task<IEnumerable<PackageResponseDtos>> SortPackages(string? sortBy);

    }
}
