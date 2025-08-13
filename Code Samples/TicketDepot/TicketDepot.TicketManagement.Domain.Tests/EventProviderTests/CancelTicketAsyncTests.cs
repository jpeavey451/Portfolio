using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using TicketDepot.TicketManagement.Domain.Tests.Factory;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain.Tests
{
    public partial class EventProviderTests
    {
        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public async Task When_ReservationId_Null_Then_ServerError(string reservationId)
        {
            string error = "reservationId";

            this.mockEventValidator.Setup(_ => _.CancelTicketsValidation(reservationId, this.transactionId, default)).ThrowsAsync(new ArgumentException(error)).Verifiable();
            this.mockObjectResultProvider.Setup(_ => _.ServerError(It.IsAny<string>())).Returns(this.objectResultProvider.ServerError()).Verifiable();

            IEventProvider sut = this.GetSystemUnderTest();

           ObjectResult result = await sut.CancelTicketsAsync(reservationId, this.transactionId, default).ConfigureAwait(false);

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

        [TestMethod]
        [DataRow("")]
        [DataRow(null)]
        public async Task When_TransactionId_Null_Then_ServerError(string transactionId)
        {
            string error = "transactionId";

            this.mockEventValidator.Setup(_ => _.CancelTicketsValidation(this.reservationId, transactionId, default)).ThrowsAsync(new ArgumentException(error)).Verifiable();

            this.mockObjectResultProvider.Setup(_ => _.ServerError(It.IsAny<string>())).Returns(this.objectResultProvider.ServerError()).Verifiable();

            IEventProvider sut = this.GetSystemUnderTest();

            ObjectResult result = await sut.CancelTicketsAsync(this.reservationId, transactionId, default).ConfigureAwait(false);

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

        [TestMethod]
        public async Task When_Validator_Returns_Null_ServerError()
        {
            string error = "A50DF9CB-6528-45D7-9892-A0F718E604B2 Unexpected Error. Reservation is null.";

            this.mockEventValidator.Setup(_ => _.CancelTicketsValidation(this.reservationId, this.transactionId, default)).ReturnsAsync(this.objectResultProvider.Ok((Reservation)null)).Verifiable();

            this.mockObjectResultProvider.Setup(_ => _.ServerError(It.IsAny<string>())).Returns(this.objectResultProvider.ServerError()).Verifiable();

            IEventProvider sut = this.GetSystemUnderTest();

            ObjectResult result = await sut.CancelTicketsAsync(this.reservationId, this.transactionId, default).ConfigureAwait(false);

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

        [TestMethod]
        public async Task When_Reserveation_Already_Cancelled_Then_Success()
        {
            Reservation reservation = DomainFactory.GetReservation();
            reservation.ReservationStatus = ReservationStatus.Cancelled;

            this.mockEventValidator.Setup(_ => _.CancelTicketsValidation(this.reservationId, this.transactionId, default)).ReturnsAsync(this.objectResultProvider.Ok(reservation)).Verifiable();

            this.mockObjectResultProvider.Setup(_ => _.Ok(It.IsAny<string>())).Returns(this.objectResultProvider.Ok).Verifiable();

            IEventProvider sut = this.GetSystemUnderTest();

            ObjectResult result = await sut.CancelTicketsAsync(this.reservationId, this.transactionId, default).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.OK);

            this.mockLogger.Verify(
                            x => x.Log(
                                It.IsAny<LogLevel>(),
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((v, t) => true),
                                It.IsAny<Exception>(),
                                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                                Times.Never);

            this.mockEventValidator.VerifyAll();
            this.mockObjectResultProviderLogger.VerifyAll();
        }

        [TestMethod]
        public async Task When_Reserveation_Purchased_Then_PreconditionFailed()
        {
            Reservation reservation = DomainFactory.GetReservation();
            reservation.ReservationStatus = ReservationStatus.Purchased;
            string preconditionFailedError = "Purchased tickets cannot be cancelled";

            this.mockEventValidator.Setup(_ => _.CancelTicketsValidation(this.reservationId, this.transactionId, default)).ReturnsAsync(this.objectResultProvider.Ok(reservation)).Verifiable();

            this.mockObjectResultProvider.Setup(_ => _.PreconditionFailed(It.IsAny<string>())).Returns(this.objectResultProvider.PreconditionFailed(preconditionFailedError)).Verifiable();

            IEventProvider sut = this.GetSystemUnderTest();

            ObjectResult result = await sut.CancelTicketsAsync(reservationId, transactionId, default).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.PreconditionFailed);
            Assert.IsTrue(result.Value.ToString().Equals(preconditionFailedError, StringComparison.Ordinal));

            this.mockLogger.Verify(
                            x => x.Log(
                                It.IsAny<LogLevel>(),
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((v, t) => true),
                                It.IsAny<Exception>(),
                                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                                Times.Never);

            this.mockEventValidator.VerifyAll();
            this.mockObjectResultProviderLogger.VerifyAll();
        }

        [TestMethod]
        public async Task When_Reserveation_Reserved_Then_Success()
        {
            Reservation reservation = DomainFactory.GetReservation();
            reservation.ReservationStatus = ReservationStatus.Reserved;

            this.mockEventValidator.Setup(_ => _.CancelTicketsValidation(this.reservationId, this.transactionId, default)).ReturnsAsync(this.objectResultProvider.Ok(reservation)).Verifiable();

            this.mockReservationRepository.Setup(_ => _.UpdateAsync(this.reservationId, reservation, default)).ReturnsAsync(this.objectResultProvider.Ok(reservation)).Verifiable();

            IEventProvider sut = this.GetSystemUnderTest();

            ObjectResult result = await sut.CancelTicketsAsync(reservationId, transactionId, default).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.OK);
            Assert.IsTrue(result.Value is Reservation);

            this.mockLogger.Verify(
                            x => x.Log(
                                It.IsAny<LogLevel>(),
                                It.IsAny<EventId>(),
                                It.Is<It.IsAnyType>((v, t) => true),
                                It.IsAny<Exception>(),
                                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                                Times.Never);

            this.mockEventValidator.VerifyAll();
            this.mockReservationRepository.VerifyAll();
            this.mockObjectResultProvider.VerifyAll();
        }
    }
}
