namespace XVP.Service.Initializers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;

    using Newtonsoft.Json;

    using XVP.Domain.DataObjects;
    using XVP.Infrastructure.Logging;
    using XVP.Repositories.Repositories;
    using XVP.Service.Services;

    public static class DocumentDbInitializer<TDocument> where TDocument : SerializableResource
    {
        public static async Task<bool> Initialize(string seed, string collection = "DemoCollection", string database = "DemoDB", string deleteCacheKey = "DocumentService.GetAll()")
        {
            var success = false;
            try
            {
                await SeedCollectionAsync(seed, collection, database, deleteCacheKey);
                success = true;
            }
            catch (DocumentClientException de)
            {
                var baseException = de.GetBaseException();
                ElmahLogger.LogError(baseException);
            }
            catch (Exception e)
            {
                var baseException = e.GetBaseException();
                ElmahLogger.LogError(baseException);
            }

            return success;
        }

        private static async Task SeedCollectionAsync(string seedList, string collection, string database, string deleteCacheKey)
        {
            try
            {
                //if (CacheService.Cache.KeyExists(deleteCacheKey))
                //{
                //    CacheService.Cache.KeyDelete(deleteCacheKey);
                //}
                var repository = new DocumentCommandRepository<TDocument>(collection, database);
                {
                    var seeds = JsonConvert.DeserializeObject<List<TDocument>>(seedList);
                    foreach (var seed in seeds)
                    {
                        await repository.InsertAsync(seed);
                    }
                }
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
            }
        }
    }
}