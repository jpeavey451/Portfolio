using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using TicketDepo.TicketManagement.Clients.Interfaces;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;
using Validation;

namespace TicketDepo.TicketManagement.Clients
{
    /// <summary>
    /// The Payment Service Client class.
    /// </summary>
    public partial class PaymentServiceClient : IPaymentServiceClient
    {
        private readonly HttpClient httpClient;
        private readonly IObjectResultProvider objectResultProvider;
        private readonly IClientProvider clientProvider;

        /// <summary>
        /// Initializes a new instance of <see cref="PaymentServiceClient"/>.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="objectResultProvider"></param>
        /// <param name="clientProvider"></param>
        public PaymentServiceClient(
            HttpClient httpClient,
            IObjectResultProvider objectResultProvider,
            IClientProvider clientProvider)
        {
            this.httpClient = httpClient;
            this.objectResultProvider = objectResultProvider;
            this.clientProvider = clientProvider;
        }

        /// <inheritdoc/>
        public async Task<ObjectResult> ProcessPayment(string apiversion, string ifMatch, PaymentInfo body, CancellationToken cancellationToken = default)
        {
                Requires.NotNullOrWhiteSpace(apiversion, nameof(apiversion));
                Requires.NotNull(body, nameof(body));

                StringBuilder urlBuilder = new StringBuilder(100);
                urlBuilder.Append("api/processpayment?");
                urlBuilder.Append(Uri.EscapeDataString("api-version") + "=").Append(Uri.EscapeDataString(this.clientProvider.ConvertToString(apiversion, CultureInfo.InvariantCulture))).Append("&");
                urlBuilder.Length--;

            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(body, Serializer.SerializerSettings));
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request.Content = content;
                request.Method = new HttpMethod("POST");
                request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));


                string url = urlBuilder.ToString();
                request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);


                HttpResponseMessage response = await this.httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                Dictionary<string, IEnumerable<string>> headers = Enumerable.ToDictionary(response.Headers, h => h.Key, h => h.Value);
                if (response.Content != null && response.Content.Headers != null)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> item in response.Content.Headers)
                        headers[item.Key] = item.Value;
                }

                int status = (int)response.StatusCode;
                if (status == 202)
                {
                    ObjectResponseResult<string> objectResponse = await this.clientProvider.ReadObjectResponseAsync<string>(response, headers, false, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                    }
                    return this.objectResultProvider.Accepted(objectResponse.Object);
                }
                else
                if (status == 404)
                {
                    ObjectResponseResult<JToken> objectResponse = await this.clientProvider.ReadObjectResponseAsync<JToken>(response, headers, false, cancellationToken).ConfigureAwait(false);
                    if (objectResponse.Object == null)
                    {
                        throw new ApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                    }

                    return this.objectResultProvider.ResourceNotFound("Customer does not exist.");
                }
                else
                if (status == 401)
                {
                    string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return this.objectResultProvider.UnAuthorized("Request is not authorized.");
                }
                else
                if (status == 400)
                {
                    string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return this.objectResultProvider.BadRequest("The request has malformed input.");
                }
                else
                if (status == 500)
                {
                    string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return this.objectResultProvider.ServerError();
                }
                else
                if (status == 403)
                {
                    string responseText = (response.Content == null) ? string.Empty : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return this.objectResultProvider.Forbidden();
                }
                else
                {
                    string? responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    throw new ApiException("The HTTP status code of the response was not expected (" + status + ").", status, responseData!, headers, null!);
                }
            }
        }
    }
}
