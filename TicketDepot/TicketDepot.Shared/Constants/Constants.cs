namespace TicketDepot.Shared
{
    /// <summary>
    /// Constants class
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The Max Depth for Newtonsoft Json Serialization.
        /// Setting this to match .Net serialization.
        /// https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions.maxdepth?view=net-7.0
        /// </summary>
        public const int NewtonSoftJsonSerializerMaxDepth = 64;

        public const string EnvConfigPrefix = "TicketManagement_";
    }
}
