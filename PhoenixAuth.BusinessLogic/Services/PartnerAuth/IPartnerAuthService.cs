using System.Threading.Tasks;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.PartnerAuth
{
    public interface IPartnerAuthService
    {
        Task<BusinessEntities.Models.Model.PartnerAuth> GetPartnerAuthAsync(string clientId);
        Task<ResponseResult> InsertPartnerAuthAsync(BusinessEntities.Models.Model.PartnerAuth model);
        Task<ResponseResult> UpdatePartnerAuthAsync(BusinessEntities.Models.Model.PartnerAuth model);
    }
}
