using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using TicketDepot.Shared;

namespace TicketDepo.TicketManagement.Clients
{
    /// <summary>
    /// Provider class for Clients.
    /// </summary>
    public class ClientProvider : IClientProvider
    {

        public async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers, bool ReadResponseAsString = false, CancellationToken cancellationToken = default)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default!, string.Empty);
            }

            if (ReadResponseAsString)
            {
                string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                try
                {
                    T? typedBody = JsonConvert.DeserializeObject<T>(responseText, Serializer.SerializerSettings);
                    return new ObjectResponseResult<T>(typedBody!, responseText);
                }
                catch (JsonException exception)
                {
                    string message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, responseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    using (StreamReader streamReader = new StreamReader(responseStream))
                    using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                    {
                        JsonSerializer serializer = JsonSerializer.Create(Serializer.SerializerSettings);
                        T? typedBody = serializer.Deserialize<T>(jsonTextReader);
                        return new ObjectResponseResult<T>(typedBody!, string.Empty);
                    }
                }
                catch (JsonException exception)
                {
                    string message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        public string ConvertToString(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "";
            }

            if (value is Enum)
            {
                string? name = Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    FieldInfo? field = IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        EnumMemberAttribute? attribute = CustomAttributeExtensions.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }

                    string? converted = Convert.ToString(Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    return converted == null ? string.Empty : converted;
                }
            }
            else if (value is bool)
            {
                return Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return Convert.ToBase64String((byte[])value);
            }
            else if (value.GetType().IsArray)
            {
                IEnumerable<object> array = Enumerable.OfType<object>((Array)value);
                return string.Join(",", Enumerable.Select(array, o => ConvertToString(o, cultureInfo)));
            }

            string? result = Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
        }
    }
}
