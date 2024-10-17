using System.Text;
using System.Text.Json;

namespace KeyStone.Web.Extensions
{
    public static class JsonExtensions
    {
        private static JsonSerializerOptions _serializerOptions;
        static JsonExtensions()
        {
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        public static T? Deserialize<T>(string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }
                T? result = JsonSerializer.Deserialize<T>(content, _serializerOptions) ?? default;
                return result;
            }
            catch
            {
                return default;
            }
        }

        public static string ToJson(this object content)
        {
            try
            {
                if (content is null)
                {
                    return string.Empty;
                }
                string result = JsonSerializer.Serialize(content, _serializerOptions);
                return result;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToBase64(this object obj)
        {
            try
            {
                string json = JsonSerializer.Serialize(obj);
                byte[] bytes = Encoding.Default.GetBytes(json);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static T? FromBase64<T>(this string base64Text)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64Text);
                string json = Encoding.Default.GetString(bytes);
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public static T? ToObjectFromJson<T>(this string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                {
                    return default;
                }
                T? result = JsonSerializer.Deserialize<T>(content, _serializerOptions) ?? default;
                return result;
            }
            catch
            {
                return default;
            }
        }
    }
}