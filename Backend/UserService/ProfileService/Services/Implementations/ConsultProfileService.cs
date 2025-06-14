using ProfileService.DAO;
using ProfileService.Models;
using ProfileService.Services.Intefaces;

namespace ProfileService.Services.Implements {
    public class ConsultProfileService : IConsultProfileService {
        private readonly ProfileDAO _profileDAO;
        private readonly ConsultUserDAO _userDAO;

        public ConsultProfileService (ProfileDAO profileDAO, ConsultUserDAO userDAO) {
            _profileDAO = profileDAO;
            _userDAO = userDAO;
        }

        public async Task<MessageResponse<MyPersonalInformationDTO>> GetMyDataInformation (string username) {
            MessageResponse<Entities.User> response = await _profileDAO.GetMyPersonalInformationUserAsync (username);

            if (response.IsError)
                return MessageResponse<MyPersonalInformationDTO>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<MyPersonalInformationDTO>.Success (response.Message, default);

            string? role = await _userDAO.GetRoleUserAsync (username);
            role = string.IsNullOrEmpty (role) ? "Sin Role asignado." : role;
            MyPersonalInformationDTO personalInformation = new MyPersonalInformationDTO {
                Username = username,
                FullName = $"{response.DataRetrieved.FirstName} {response.DataRetrieved.MiddleName} {response.DataRetrieved.LastName}",
                Email = response.DataRetrieved.Email,
                AreaCode = response.DataRetrieved.AreaCode,
                PhoneNumber = response.DataRetrieved.PhoneNumber,
                Role = role
            };
            return MessageResponse<MyPersonalInformationDTO>.Success (response.Message, personalInformation);
        }

        public async Task<MessageResponse<List<AddressDTO>>> GetAddressesAsync (string username) {
            MessageResponse<List<Entities.Address>> response = await _profileDAO.GetAddressesUserAsync (username);

            if (response.IsError)
                return MessageResponse<List<AddressDTO>>.Failure (response.Message);
            if (response.DataRetrieved == null)
                return MessageResponse<List<AddressDTO>>.Success (response.Message, default);
            if (response.DataRetrieved.Count <= 0)
                return MessageResponse<List<AddressDTO>>.Success ("Usuario sin direcciones registradas.", null);

            List<AddressDTO> addresses = response.DataRetrieved
                .Select (addr => new AddressDTO {
                    Street = addr.Street,
                    NumberExterior = addr.ExtNumber,
                    NumberInterior = addr.IntNumber,
                    Neighborhood = addr.Neighborhood,
                    City = addr.City,
                    PostalCode = addr.PostalCode,
                    State = addr.State,
                    Country = addr.Country,
                    IsActive = addr.User_Addresses.FirstOrDefault ()?.IsActive ?? false // TODO
                }).ToList ();
            return MessageResponse<List<AddressDTO>>.Success (response.Message, addresses);
        }
    }
}
