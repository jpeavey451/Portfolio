using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain.Tests
{
    public partial class EventProviderTests
    {
        [TestMethod]
        public async Task When_Validator_Event_Null_ServerError()
        {
            string error = "newEvent";

            this.mockEventValidator.Setup(_ => _.ValidateNewEvent(null!, default)).ThrowsAsync(new ArgumentNullException(error));
            this.mockObjectResultProvider.Setup(_ => _.ServerError(It.IsAny<string>())).Returns(this.objectResultProvider.ServerError());

            IEventProvider sut = this.GetSystemUnderTest();

            ObjectResult result = await sut.CreateEventAsync(null!, default).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.InternalServerError);

            this.mockLogger.Verify(
                            x => x.Log(
                                It.IsAny<LogLevel>(),
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((v, t) => true),
                                It.IsAny<Exception>(),
                                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                                Times.Once);

            this.mockEventValidator.VerifyAll();
            this.mockObjectResultProviderLogger.VerifyAll();
        }
    }
}
