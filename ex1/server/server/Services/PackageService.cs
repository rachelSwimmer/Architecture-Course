using server.DTOs;
using server.Interfaces;
using server.Models;
using server.Repositories;

namespace server.Services
{
    public class PackageService:IPackageService
    {
        private readonly IPackageRepository _packageRepository;
        private readonly ILogger<PackageService> _logger;
        public PackageService(IPackageRepository packageRepository, ILogger<PackageService> logger)
        {
            _packageRepository = packageRepository;
            _logger = logger;
        }
        public async Task<PackageResponseDtos> AddPackage(PackageCreateDtos packageDto)
        {
            _logger.LogInformation("Post /add package called");
            try
            {
                var package = new Package
                {
                    Name = packageDto.Name,
                    Description = packageDto.Description,
                    Quentity = packageDto.Quentity,
                    Price = packageDto.Price,
                };
                var createdPackage = await _packageRepository.AddPackage(package);
                return MapToResponeseDto(createdPackage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding package");
                throw;
            }
            
        }

        public async Task<bool> DeletePackage(int id)
        {
            _logger.LogInformation("Post / delete package {packageId} deleted", id);
            try
            {
                return await _packageRepository.DeletePackage(id);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting package");
                throw;
            }
           
        }

        public async Task<IEnumerable<PackageResponseDtos>> GetAll()
        {
            _logger.LogInformation("Get / all package called");
            try
            {
                var Package = await _packageRepository.GetAll();
                return Package.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving packages");
                throw;
            }
           
        }

        public async Task<PackageResponseDtos> GetById(int id)
        {
            _logger.LogInformation("Get / get package : {packageId} called", id);
            try
            {
                var package = await _packageRepository.GetById(id);
                return package != null ? MapToResponeseDto(package) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving package by ID");
                throw;
            }
           
        }
        public async Task<PackageResponseDtos> UpdatePackage(int packageId, PackageUpdateDtos packageDto)
        {
            _logger.LogInformation("PUT / update package : {packageId} called", packageId);
            try
            {
                var existingPackage = await _packageRepository.GetById(packageId);
                if (existingPackage == null)
                    return null;
                existingPackage.Name = packageDto.Name;
                existingPackage.Description = packageDto.Description;
                existingPackage.Quentity = packageDto.Quentity;
                existingPackage.Price = packageDto.Price;
                var updatedCategory = await _packageRepository.UpdatePackage(existingPackage);
                return MapToResponeseDto(updatedCategory);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating package");
                throw;
            }
            
        }
        public async Task<IEnumerable<PackageResponseDtos>> SortPackages(string? sortBy)
        {
            _logger.LogInformation("Get / sort package called");
            try
            {
                var packages = await _packageRepository.SortPackages(sortBy);
                return packages.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sort package");
                throw;
            }
           

        }

        private static PackageResponseDtos MapToResponeseDto(Package package)
        {
            return new PackageResponseDtos
            {
                Id = package.Id,
                Name = package.Name,
                Description = package.Description,
                Quentity = package.Quentity,
                Price = package.Price
            };

        }
       
    }
}
