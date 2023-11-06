using MangoWeb.Models;

namespace MangoWeb.Service.IService;

public interface IBaseService
{
   Task<ResponseDTO?> SendAsync(RequestDTO requestDto);
}