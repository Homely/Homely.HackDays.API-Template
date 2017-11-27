using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace API.CRUD
{
    public static class JsonHelpers
    {
        private static readonly Lazy<JsonSerializerSettings> LazyJsonSerializerSettings = new Lazy<JsonSerializerSettings>(() =>
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.None,
                NullValueHandling = NullValueHandling.Include,
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };

            settings.Converters.Add(new StringEnumConverter());
            return settings;
        });

        public static JsonSerializerSettings JsonSerializerSettings => LazyJsonSerializerSettings.Value;
    }
}