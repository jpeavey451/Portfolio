using Microsoft.AspNetCore.Mvc;

namespace TicketDepot.Shared
{
    public interface IValidationProvider
    {
        /// <summary>
        /// Validates the properties via data annotations.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>The <see cref="ObjectResult"/>indicating success or failure.</returns>
        ObjectResult ValidateObject(object obj);
    }
}
