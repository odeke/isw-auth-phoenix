using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static PhoenixAuth.BusinessLogic.Models.UserMessages;
using PhoenixAuth.BusinessLogic.BusinessEntities;
using PhoenixAuth.BusinessLogic.Extensions;
using PhoenixAuth.BusinessLogic.Models;

namespace PhoenixAuth.BusinessLogic.Services.PartnerAuth
{
    public class PartnerAuthService : IPartnerAuthService
    {
        private readonly PhoenixAuthDbContext _dbContext;

        public PartnerAuthService(PhoenixAuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BusinessEntities.Models.Model.PartnerAuth> GetPartnerAuthAsync(string clientId)
        {

            var client = await _dbContext.PartnerAuths.FirstOrDefaultAsync(p => p.ClientId == clientId);

            if (client.IsNull()) throw new Exception(string.Format(NotFound, nameof(client)));

            return client;
        }

        public async Task<ResponseResult> InsertPartnerAuthAsync(BusinessEntities.Models.Model.PartnerAuth model)
        {
            await _dbContext.PartnerAuths.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            return new ResponseResult { Response = model };
        }

        public async Task<ResponseResult> UpdatePartnerAuthAsync(BusinessEntities.Models.Model.PartnerAuth model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return new ResponseResult { Response = model };
        }

        //public async Task<ResponseResult> DeletePartnerAuthAsync(long id)
        //{
        //    return await _crudCoreService.DeleteModelAsync<BusinessEntities.Models.Model.PartnerAuth>(p => p.Id == id), id,
        //        "Item Category with the specified Id is not found or the category already has items attached to it.");
        //}

    }
}
