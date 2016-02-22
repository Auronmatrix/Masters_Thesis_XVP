namespace XVP.Infrastructure.Command.Initializers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AzureSearchClient;

    using Newtonsoft.Json;

    using XVP.Infrastructure.Command.Repositories;
    using XVP.Infrastructure.Shared.Logging;

    public static class SearchDbInitializer<TDocument>
        where TDocument : Document
    {
        public static async Task Initialize(string seed, string indexName)
        {
            SearchInitResponse result = null;
            try
            {
              await SeedCollectionAsync(seed, indexName.ToLower());
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
        }

        private static async Task SeedCollectionAsync(string seedList, string indexName)
        {
            var result = new SearchInitResponse();
            try
            {
                 var commandRepository = new SeachCommandRepository<TDocument>();
                {
                    await commandRepository.AzureSearchService.LoadIndexesAsync();
                    var seeds = JsonConvert.DeserializeObject<List<TDocument>>(seedList);
                    var del = await commandRepository.DeleteIndex(indexName);
                    var insert = await commandRepository.CreateDocumentsInIndex(seeds, indexName);
                    if (!del)
                    {
                        result.MessageList.Add("Delete failed");
                    }

                    if (!insert)
                    {
                        result.MessageList.Add("Insert failed");
                    }
                }
            }
            catch (Exception ex)
            {
                ElmahLogger.LogError(ex);
            }
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