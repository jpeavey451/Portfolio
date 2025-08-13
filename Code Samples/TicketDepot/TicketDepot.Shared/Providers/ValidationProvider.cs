

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TicketDepot.Shared
{
    public class ValidationProvider : IValidationProvider
    {
        private readonly IObjectResultProvider objectResultProvider;

        public ValidationProvider(
            IObjectResultProvider objectResultProvider)
        {
            this.objectResultProvider = objectResultProvider;
        }

        public ObjectResult ValidateObject(object obj)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(obj);
            bool isValid = Validator.TryValidateObject(obj, context, results, true);

            if (!isValid)
            {
                this.objectResultProvider.BadRequest(string.Join("; ", results.Select(r => r.ErrorMessage))
                );
            }

            return this.objectResultProvider.Ok();
        }
    }
}
