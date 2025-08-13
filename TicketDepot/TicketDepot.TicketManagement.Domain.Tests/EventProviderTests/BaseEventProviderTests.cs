
using Microsoft.Extensions.Logging;
using Moq;
using TicketDepot.Shared;
using TicketDepot.TicketManagement.Repository;

namespace TicketDepot.TicketManagement.Domain.Tests
{
    [TestClass]
    [TestCategory("EventProvider")]
    public partial class EventProviderTests
    {
        string transactionId = Guid.NewGuid().ToString("N");
        string reservationId = Guid.NewGuid().ToString("N");
        string eventId = Guid.NewGuid().ToString("N");

        private Mock<ILogger<EventProvider>> mockLogger;
        private Mock<IObjectResultProvider> mockObjectResultProvider;
        private Mock<IEventRepository> mockEventRepository;
        private Mock<IReservationRepository> mockReservationRepository;
        private Mock<IPaymentProvider> mockPaymentProvider;
        private Mock<IEventValidator> mockEventValidator;
        private Mock<ILogger<ObjectResultProvider>> mockObjectResultProviderLogger;
        private IObjectResultProvider objectResultProvider;


        [TestInitialize]
        public void TestInit()
        {
            this.mockLogger = new Mock<ILogger<EventProvider>>();
            this.mockObjectResultProvider = new Mock<IObjectResultProvider>();
            this.mockEventRepository = new Mock<IEventRepository>();
            this.mockEventValidator = new Mock<IEventValidator>();
            this.mockReservationRepository = new Mock<IReservationRepository>();
            this.mockPaymentProvider = new Mock<IPaymentProvider>();
            this.mockEventValidator = new Mock<IEventValidator>();
            this.mockObjectResultProviderLogger = new Mock<ILogger<ObjectResultProvider>>();
            this.objectResultProvider = new ObjectResultProvider(this.mockObjectResultProviderLogger.Object);

        }

        private IEventProvider GetSystemUnderTest()
        {
            return new EventProvider(
                this.mockLogger.Object,
                this.mockObjectResultProvider.Object,
                this.mockEventRepository.Object,
                this.mockReservationRepository.Object,
                this.mockPaymentProvider.Object,
                this.mockEventValidator.Object);
        }

    }
}
