using Backend.DAO.User;
using Backend.DTO;
using Backend.DTO.User.Profile;
using Backend.Services.Intefaces.User;

namespace Backend.Services.Implements.User {
    public class ProfileService : IProfileService {
        private readonly ProfileDAO _profileDAO;
        private readonly UserDAO _userDAO;

        public ProfileService (ProfileDAO profileDAO, UserDAO userDAO) {
            _profileDAO = profileDAO;
            _userDAO = userDAO;
        }

        public async Task<MessageResponse<MyPersonalInformationDTO>> GetMyPersonalInformation (string username) {
            MessageResponse<Entities.User> response = await _profileDAO.GetMyPersonalInformationUser (username);

            if (response.isError)
                return MessageResponse<MyPersonalInformationDTO>.Failure (response.message);
            if (response.dataRetrieved == null)
                return MessageResponse<MyPersonalInformationDTO>.Success (response.message, default);

            string role = await _userDAO.GetRoleUserAsync (username);
            MyPersonalInformationDTO personalInformation = new MyPersonalInformationDTO {
                username = username,
                fullName = $"{response.dataRetrieved.FirstName} {response.dataRetrieved.MiddleName} {response.dataRetrieved.LastName}",
                email = response.dataRetrieved.Email,
                areaCode = response.dataRetrieved.AreaCode,
                phoneNumber = response.dataRetrieved.PhoneNumber,
                role = role
            };
            return MessageResponse<MyPersonalInformationDTO>.Success ("Información obtenida.", personalInformation);
        }

        public async Task<MessageResponse<List<AddressDTO>>> GetAddressAsync (string username) {
            MessageResponse<List<Entities.Address>> response = await _profileDAO.GetAddressesUser (username);

            if (response.isError)
                return MessageResponse<List<AddressDTO>>.Failure (response.message);
            if (response.dataRetrieved == null && response.message.Equals ("Usuario no encontrado."))
                return MessageResponse<List<AddressDTO>>.Success (response.message, default);
            if (response.dataRetrieved == null || response.dataRetrieved.Count <= 0)
                return MessageResponse<List<AddressDTO>>.Success ("Usuario sin direcciones registradas.", null);

            List<AddressDTO> addresses = response.dataRetrieved
                .Select (addr => new AddressDTO {
                    street = addr.Street,
                    numberExterior = addr.ExtNumber,
                    numberInterior = addr.IntNumber,
                    neighborhood = addr.Neighborhood,
                    city = addr.City,
                    postalCode = addr.PostalCode,
                    state = addr.State,
                    country = addr.Country,
                    isActive = addr.User_Addresses.FirstOrDefault ()?.IsActive ?? false // TODO
                }).ToList ();
            return MessageResponse<List<AddressDTO>>.Success ("Información obtenida.", addresses);
        }
    }
}
