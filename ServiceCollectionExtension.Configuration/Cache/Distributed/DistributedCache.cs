using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace ServiceCollectionExtension.Configuration.Cache
{
    public class DistributedCache : ICache
    {
        private readonly IDistributedCache _cache;
        private readonly Dictionary<Type, bool> _types = new Dictionary<Type, bool>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="cache">The currently configured cache</param>
        public DistributedCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets the model with the specified key from cache.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="key">The unique key</param>
        /// <returns>The cached model, null it wasn't found</returns>
        public T Get<T>(string key)
        {
            try
            {
                var value = _cache.Get(key);
                return Deserialize<T>(_cache.Get(key));
            }
            catch(RedisConnectionException exception)
            {
                //TODO log the error but do not throw exception. 
                //This is because cache is just an performance improvement instead of application main functionality
                //application should work without cache enabled
            }
            catch(Exception exception)
            {
                //TODO log the error but do not throw exception. 
            }
            return default(T);
        }

        /// <summary>
        /// Sets the given model in the cache.
        /// </summary>
        /// <typeparam name="T">The model type</typeparam>
        /// <param name="key">The unique key</param>
        /// <param name="value">The model</param>
        public void Set<T>(string key, T value)
        {
            try
            {
                _cache.Set(key, Serialize(value));
            }
            catch (RedisConnectionException exception)
            {
                //TODO log the error but do not throw exception. 
                //This is because cache is just an performance improvement instead of application main functionality
                //application should work without cache enabled
            }
            catch (Exception exception)
            {
                //TODO log the error but do not throw exception. 
            }
        }

        /// <summary>
        /// Removes the model with the specified key from cache.
        /// </summary>
        /// <param name="key">The unique key</param>
        public void Remove(string key)
        {
            try
            {
                _cache.Remove(key);
            }
            catch (RedisConnectionException exception)
            {
                //TODO log the error but do not throw exception. 
                //This is because cache is just an performance improvement instead of application main functionality
                //application should work without cache enabled
            }
            catch (Exception exception)
            {
                //TODO log the error but do not throw exception. 
            }
        }

        /// <summary>
        /// Serializes the given object to a byte array.
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The serialized byte array</returns>
        private byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                if (IsSerializable(obj.GetType()))
                {
                    formatter.Serialize(stream, obj);
                    return stream.ToArray();
                }

                // First, serialize the object to JSON.
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                var json = JsonConvert.SerializeObject(obj, settings);

                // Next lets convert the json to a byte array
                formatter.Serialize(stream, json);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes the byte array to an object.
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <typeparam name="T">The object type</typeparam>
        /// <returns>The deserialized object</returns>
        private T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null)
            {
                return default(T);
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                if (IsSerializable(typeof(T)))
                {
                    return (T)formatter.Deserialize(stream);
                }

                // First lets decode the byte array into a string
                var json = (string)formatter.Deserialize(stream);

                // Next deserialize the json into an object
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
        }

        private bool IsSerializable(Type type)
        {
            if (_types.TryGetValue(type, out var serializable))
            {
                return serializable;
            }

            var attr = type.GetCustomAttribute<SerializableAttribute>();
            _types[type] = attr != null;

            return attr != null;
        }
    }
}
