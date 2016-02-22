namespace XVP.Infrastructure.Shared.Cache
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using StackExchange.Redis;

    public static class CacheRepository
    {
        public static void Set(this IDatabase cache, string key, object value)
        {
            cache.StringSet(key, Serialize(value));
        }

        public static async void SetToJsonAsync(this IDatabase cache, string key, object value)
        {
           await cache.StringSetAsync(key, JsonConvert.SerializeObject(value));
        }

        public static void SetToJson(this IDatabase cache, string key, object value)
        {
            cache.StringSet(key, JsonConvert.SerializeObject(value));
        }

        public static async void SetAsync(this IDatabase cache, string key, object value)
        {
            await cache.StringSetAsync(key, Serialize(value));
        }

        public static async void SetUnorderedListAsync<T>(this IDatabase cache, string key, IEnumerable<T> values)
        {
            var enumerable = values as T[] ?? values.ToArray();
            var valueSet = enumerable.Select(i => (RedisValue)JsonConvert.SerializeObject(i)).ToArray();
            await cache.SetAddAsync(key, valueSet);
        }

        public static async void SetListAsync<T>(this IDatabase cache, string key, IEnumerable<T> values)
        {
            var enumerable = values as T[] ?? values.ToArray();
            var valueSet = enumerable.Select(i => new SortedSetEntry(JsonConvert.SerializeObject(i), 0)).ToArray();
            await cache.SortedSetAddAsync(key, valueSet);
        }

        public static void SetList<T>(this IDatabase cache, string key, IEnumerable<T> values)
        {
            var enumerable = values as T[] ?? values.ToArray();
            var valueSet = enumerable.Select(i => (RedisValue)JsonConvert.SerializeObject(i)).ToArray();
            cache.ListRightPush(key, valueSet);
        }

        public static T Get<T>(this IDatabase cache, string key)
        {
            return Deserialize<T>(cache.StringGet(key));
        }

        public static async Task<bool> DeleteIfExists(this IDatabase cache, string key)
        {
            if (cache.KeyExists(key))
            {
                return await cache.KeyDeleteAsync(key);
            }
            return true;

        }

        public static async Task<T> GetAsync<T>(this IDatabase cache, string key)
        {
            return Deserialize<T>(await cache.StringGetAsync(key));
        }

        public static object Get(this IDatabase cache, string key)
        {
            return Deserialize<object>(cache.StringGet(key));
        }

        public static object GetFromJson<T>(this IDatabase cache, string key)
        {
            var json = cache.StringGet(key);
            if (json.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

            return null;
        }

        public static async Task<object> GetFromJsonAsync<T>(this IDatabase cache, string key)
        {
            var json = await cache.StringGetAsync(key);
            if (json.HasValue)
            {
                return JsonConvert.DeserializeObject<T>(json);
            }

            return json;
        }

        public static object GetUnorderedListFromJson<T>(this IDatabase cache, string key)
        {
            // Find more effective method for returning list, iterating through the list and serializing it might not be the most effective
            var resultArray = cache.SetMembers(key).ToArray();
            return !resultArray.Any() ? null : resultArray.Select(result => JsonConvert.DeserializeObject<T>(result.ToString())).ToList();
        }

        public static object GetListFromJson<T>(this IDatabase cache, string key)
        {
            // Same as unordered list, find method for retrieving list or sortedList
            var resultArray = cache.ListRange(key).ToArray();
            return !resultArray.Any() ? null : resultArray.Select(result => JsonConvert.DeserializeObject<T>(result.ToString())).ToList();
        }

        private static byte[] Serialize(object o)
        {
            if (o == null)
            {
                return null;
            }

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, o);
                var objectDataAsStream = memoryStream.ToArray();
                return objectDataAsStream;
            }
        }

        private static T Deserialize<T>(byte[] stream)
        {
            var binaryFormatter = new BinaryFormatter();
            if (stream == null)
            {
                return default(T);
            }

            using (var memoryStream = new MemoryStream(stream))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
    }
}
