namespace XVP.Service.Initializers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using Newtonsoft.Json;

    using XVP.Infrastructure.Logging;
    using XVP.Repositories.Repositories;
    using XVP.Service.Services;

    public static class SearchDbInitializer<TDocument>
        where TDocument : Document
    {
        public static async Task<SearchInitResponse> Initialize(string seed, string indexName)
        {
            SearchInitResponse result = null;
            try
            {
                result = await SeedCollectionAsync(seed, indexName.ToLower());
            }
            catch (AzureSearchException de)
            {
                var baseException = de.GetBaseException();
                ElmahLogger.LogError(baseException);
            }
            catch (Exception e)
            {
                var baseException = e.GetBaseException();
                ElmahLogger.LogError(baseException);
            }

            return result;
        }

        private static async Task<SearchInitResponse> SeedCollectionAsync(string seedList, string indexName)
        {
            var result = new SearchInitResponse();
            try
            {
                var queryRepository = new SearchQueryRepository<TDocument>();

                var commandRepository = new SeachCommandRepository<TDocument>();
                 {
                    await queryRepository.LoadIndexesAsync();
                    var seeds = JsonConvert.DeserializeObject<List<TDocument>>(seedList);
                    var del = await commandRepository.DeleteIndex(indexName);
                    var insert = await commandRepository.CreateDocumentsInIndex(seeds, indexName);
                    var stats = await queryRepository.GetIndexStatistics(indexName);
                    var indexes = await queryRepository.GetIndexes();

                    if (!del)
                    {
                        result.MessageList.Add("Delete failed");
                    }

                    if (!insert)
                    {
                        result.MessageList.Add("Insert failed");
                    }

                    result.StorageSize = stats.StorageSize.ToString(CultureInfo.InvariantCulture);
                    result.DocumentCount = stats.DocumentCount.ToString(CultureInfo.InvariantCulture);
                    result.Indexes = indexes.ToList().Select(x => x.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
            }

            return result;
        }
    }

    public class SearchInitResponse
    {
        public SearchInitResponse()
        {
            this.MessageList = new List<string>();
            this.Indexes = new List<string>();
        }

        public List<string> MessageList { get; set; }

        public IEnumerable<string> Indexes { get; set; }

        public bool Success { get; set; }

        public string DocumentCount { get; set; }

        public string StorageSize { get; set; }
        }
}