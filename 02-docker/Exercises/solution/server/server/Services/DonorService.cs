using server.DTOs;
using server.Interfaces;
using server.Models;

namespace server.Services
{
    public class DonorService:IDonorService
    {
        private readonly IDonorRepository _donorRepository;
        private readonly ILogger<DonorService> _logger;
        public DonorService(IDonorRepository donorRepository, ILogger<DonorService> logger)
        {
            _donorRepository = donorRepository;
            _logger = logger;
        }
        public async Task<DonorResponseDto> AddDonor(DonorCreateDto donorDto)
        {
            _logger.LogInformation("Post /add donor called");

            var donor = new Donor
            {
                Id = donorDto.Id,
                Name = donorDto.Name,
                PhoneNumber = donorDto.PhoneNumber,
                Email = donorDto.Email,
                LogoUrl = donorDto.LogoUrl

            };
            try
            {
                var createdDonor = await _donorRepository.AddDonor(donor);
                return MapToResponeseDto(createdDonor);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while adding donor");
                throw;
            }
           
        }

        public async Task<bool> DeleteDonor(string id)
        {
            _logger.LogInformation("Post / delete donor {donorId} deleted", id);
            try
            {
                return await _donorRepository.DeleteDonor(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting donor");
                throw;
            }
        }

        public async Task<IEnumerable<DonorResponseDto>> GetAll()
        {
            _logger.LogInformation("Get / all donor called");
            try
            {
                var donors = await _donorRepository.GetAll();
                return donors.Select(MapToResponeseDto);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while retrieving donors");
                throw;
            }
            
        }

        public async Task<DonorResponseDto> GetById(string id)
        {
            _logger.LogInformation("Get / get donor : {donorId} called", id);
            try
            {
                var donor = await _donorRepository.GetById(id);
                return donor != null ? MapToResponeseDto(donor) : null;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while retrieving donor by ID");
                throw;
            }
            
        }
        public async Task<DonorResponseDto> UpdateDonor(string donorId, DonorUpdateDto donorDto)
        {
            _logger.LogInformation("PUT / update donor : {donorId} called", donorId);
            try
            {
                var existingDonor = await _donorRepository.GetById(donorId);
                if (existingDonor == null)
                    return null;
                existingDonor.Id = donorDto.Id;
                existingDonor.Name = donorDto.Name;
                existingDonor.PhoneNumber = donorDto.PhoneNumber;
                existingDonor.Email = donorDto.Email;
                existingDonor.LogoUrl = donorDto.LogoUrl;
                var updatedDonor = await _donorRepository.UpdateDonor(existingDonor);
                return MapToResponeseDto(updatedDonor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating donor");
                throw;
            }

           
        }
        public async Task<IEnumerable<DonorResponseDto>> FilterDonors(string? name, string? email, int? giftId)
        {
            _logger.LogInformation("Get / filter donor called");
            try
            {
                var donors = await _donorRepository.FilterDonors(name, email, giftId);
                return donors.Select(MapToResponeseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while filter donor");
                throw;
            }

           
        }



        private static DonorResponseDto MapToResponeseDto(Donor donor)
        {
            return new DonorResponseDto
            {
                Id = donor.Id,
                Name = donor.Name,
                PhoneNumber= donor.PhoneNumber,
                Email = donor.Email,
                LogoUrl = donor.LogoUrl
            };
        }
    }
}
