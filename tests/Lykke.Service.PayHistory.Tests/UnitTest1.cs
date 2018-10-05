using System;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.PayHistory.Client;
using Lykke.Service.PayHistory.Client.AutorestClient.Models;
using Lykke.Service.PayHistory.Client.Publisher;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Lykke.Service.PayHistory.Tests
{
    public class UnitTest1
    {
        //[Fact]
        //public async Task TestPublisher()
        //{
        //    try
        //    {
        //        var log = new Mock<ILog>();
        //        using (var publisher = new HistoryOperationPublisher(new RabbitMqPublisherSettings
        //        {
        //            ConnectionString = "amqp://lykke.history:lykke.history@rabbit-me.lykke-me.svc.cluster.local:5672",
        //            ExchangeName = "payhistory.operations"
        //        }, log.Object))
        //        {
        //            publisher.Start();

        //            await publisher.PublishAsync(new HistoryOperation
        //            {
        //                Type = HistoryOperationType.CashOut,
        //                OppositeMerchantId = "Merchant 1",
        //                CreatedOn = DateTime.Now,
        //                Amount = 100,
        //                AssetId = "USD",
        //                DesiredAssetId = "CHF",
        //                MerchantId = "Merchant 2",
        //                InvoiceId = "123456",
        //                InvoiceStatus = "Underpaid",
        //                EmployeeEmail = "test@test.ru",
        //                TxHash = "1234567890"
        //            });

        //            Thread.Sleep(100);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var a = 1;
        //    }
        //}

        //[Fact]
        //public void TestClient()
        //{
        //    var client = new PayHistoryClient("http://localhost:5000/");
        //    var result = client.GetDetails("a0b7ec73-380d-4d9d-aa9c-692f5d7aa477");
        //}
    }

}
