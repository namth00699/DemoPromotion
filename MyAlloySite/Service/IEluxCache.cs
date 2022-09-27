using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace MyAlloySite.Service
{
    interface IEluxCache
    {
        T Get<T>(string key);

        T GetOrFetch<T>(string key, Func<T> fetch, TimeSpan offset);
        T GetOrFetch<T>(string key, Func<T> fetch, TimeSpan expiration, string masterKey);
        T GetOrFetch<T>(string key, Func<T> fetch, TimeSpan expiration, string[] masterKey);

        void Add<T>(string key, T objectToCache, TimeSpan expiration);
        void Add<T>(string key, T objectToCache, TimeSpan expiration, string masterKey);
        void Add<T>(string key, T objectToCache, TimeSpan expiration, string[] masterKey);
        void Add<T>(string key, T objectToCache, TimeSpan expiration, IEnumerable<ContentReference> dependentContents, string[] masterKeys);
        void Add<T>(string key, T objectToCache, CacheEvictionPolicy policy);
        void Add<T>(string key, T objectToCache, IEnumerable<ContentReference> dependentContents);
        void Add<T>(string key, T objectToCache, IEnumerable<ContentReference> dependentContents, string[] masterKeys);

        void Remove(string key);
        bool Exists(string key);
        string BuildCacheKey(Dictionary<string, object> nameAndValueOfParameters, string nameOfClass, [CallerMemberName] string nameOfMethod = "");
    }

    [ServiceConfiguration(typeof(IEluxCache), Lifecycle = ServiceInstanceScope.Singleton)]
    public class EluxCache : IEluxCache
    {
        private readonly IContentCacheKeyCreator _contentCacheKey;

        public EluxCache(IContentCacheKeyCreator contentCacheKey)
        {
            _contentCacheKey = contentCacheKey;
        }

        public T Get<T>(string key)
        {
            T cacheItem;

            try
            {
                cacheItem = (T)CacheManager.Get(key);
            }
            catch (Exception)
            {
                cacheItem = default(T);
            }

            return cacheItem;
        }

        public virtual T GetOrFetch<T>(string key, Func<T> fetch, TimeSpan offset)
        {
            if (Exists(key))
            {
                return Get<T>(key);
            }

            try
            {
                var result = fetch();
                Add(key, result, offset);
                return result;
            }
            catch
            {
                Add(key, default(T), TimeSpan.FromSeconds(30));
                return default(T);
            }
        }

        public virtual T GetOrFetch<T>(string key, Func<T> fetch, TimeSpan expiration, string masterKey)
        {
            if (Exists(key))
            {
                return Get<T>(key);
            }

            try
            {
                var result = fetch();
                Add(key, result, expiration, masterKey);
                return result;
            }
            catch
            {
                Add(key, default(T), TimeSpan.FromSeconds(30));
                return default(T);
            }
        }

        public virtual T GetOrFetch<T>(string key, Func<T> fetch, TimeSpan expiration, string[] masterKeys)
        {
            if (Exists(key))
            {
                return Get<T>(key);
            }

            try
            {
                var result = fetch();
                Add(key, result, expiration, masterKeys);
                return result;
            }
            catch
            {
                Add(key, default(T), TimeSpan.FromSeconds(30));
                return default(T);
            }
        }

        public void Add<T>(string key, T objectToCache, TimeSpan expiration)
        {
            var cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute);
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public void Add<T>(string key, T objectToCache, TimeSpan expiration, string masterKey)
        {
            var masterKeys = new[] { masterKey };

            var cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute, null, masterKeys);
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public void Add<T>(string key, T objectToCache, TimeSpan expiration, string[] masterKeys)
        {
            var cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute, null, masterKeys);
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public void Add<T>(string key, T objectToCache, TimeSpan expiration, IEnumerable<ContentReference> dependentContents, string[] masterKeys)
        {
            var cacheEvictionPolicy = new CacheEvictionPolicy(expiration, CacheTimeoutType.Absolute, null, dependentContents.Select(cotnentReference => _contentCacheKey.CreateCommonCacheKey(cotnentReference)).Concat(masterKeys));
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public void Add<T>(string key, T objectToCache, CacheEvictionPolicy cacheEvictionPolicy)
        {
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public bool Exists(string key)
        {
            return (CacheManager.Get(key) != null);
        }

        public void Remove(string key)
        {
            CacheManager.Remove(key);
        }

        public void Add<T>(string key, T objectToCache, IEnumerable<ContentReference> dependentContents)
        {
            var cacheEvictionPolicy = new CacheEvictionPolicy(dependentContents.Select(cotnentReference => _contentCacheKey.CreateCommonCacheKey(cotnentReference)));
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public void Add<T>(string key, T objectToCache, IEnumerable<ContentReference> dependentContents, string[] masterKeys)
        {
            var cacheEvictionPolicy = new CacheEvictionPolicy(dependentContents.Select(cotnentReference => _contentCacheKey.CreateCommonCacheKey(cotnentReference)), masterKeys);
            CacheManager.Insert(key, objectToCache, cacheEvictionPolicy);
        }

        public string BuildCacheKey(Dictionary<string, object> nameAndValueOfParameters, string nameOfClass, [CallerMemberName] string nameOfMethod = "")
        {
            var underscore = "_";

            var cacheKeyBuilder = new StringBuilder();

            cacheKeyBuilder
                .Append(underscore)
                .Append(nameOfClass)
                .Append(underscore)
                .Append(nameOfMethod)
                .Append(underscore);

            foreach (var nameAndValueOfParameter in nameAndValueOfParameters)
            {
                cacheKeyBuilder.Append(nameAndValueOfParameter.Key);
                cacheKeyBuilder.Append(underscore);

                if (nameAndValueOfParameter.Value == null)
                {
                    cacheKeyBuilder.Append(string.Empty);
                }
                else
                {
                    cacheKeyBuilder.Append(nameAndValueOfParameter.Value);
                }
            }

            return BuildContextureCacheKey(cacheKeyBuilder.ToString());
        }

        public static string BuildContextureCacheKey(string originalKey)
        {
            var context = (System.Web.HttpContext.Current?.User?.Identity == null
                        || !System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
                ? "Anonymous"
                : "Authenticated";
            return string.Format("context:{0}_{1}", context, originalKey);
        }
    }
}
