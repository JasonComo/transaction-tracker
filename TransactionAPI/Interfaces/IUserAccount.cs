using TransactionAPI.Dto;
using static TransactionAPI.Dto.ServiceResponses;

namespace TransactionAPI.Interfaces
{
    public interface IUserAccount
    {
        Task<GeneralResponse> CreateAccount(UserDTO userDTO);
        Task<LoginResponse> LoginAccount(LoginDTO loginDTO);
        Task<string> GetUserNameById(string userId);
    }
}
