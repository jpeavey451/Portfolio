using Newtonsoft.Json;

namespace TicketDepot.Shared
{
    public static class Serializer
    {
        private static List<string> errors = new List<string>();

        /// <summary>
        /// Gets the errors string list.
        /// </summary>
        public static List<string> Errors => errors;

        /// <summary>
        /// Gets and Sets the serializer settings.
        /// </summary>
        public static JsonSerializerSettings SerializerSettings { get; set; } = new JsonSerializerSettings()
        {
            // setting this to match .Net serialization
            // https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions.maxdepth?view=net-7.0
            MaxDepth = Constants.NewtonSoftJsonSerializerMaxDepth,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Populate,
            Error = (sender, args) =>
            {
                errors.Add(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = false;
            },
        };

        /// <summary>
        /// Object extension for serialization.
        /// </summary>
        /// <param name="objectToSerialize"></param>
        /// <returns></returns>
        public static string SerializeObject(this object objectToSerialize)
        {
            errors = new List<string>();
            return JsonConvert.SerializeObject(objectToSerialize, SerializerSettings);
        }

        /// <summary>
        /// String extension for deserialization.
        /// </summary>
        /// <typeparam name="T">The type to deserialize into.</typeparam>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string stringValue)
        {
            errors = new List<string>();
            return JsonConvert.DeserializeObject<T>(stringValue, SerializerSettings);
        }

        public static T TransformObject<T>(this object objectToSerialize)
        {
            errors = new List<string>();
            string stringValue = JsonConvert.SerializeObject(objectToSerialize, SerializerSettings);
            return JsonConvert.DeserializeObject<T>(stringValue, SerializerSettings);
        }
    }
}
