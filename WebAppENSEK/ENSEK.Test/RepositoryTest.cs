using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using WebAppENSEK.Controllers;
using WebAppENSEK.Model;
using WebAppENSEK.Services;
using Xunit;

namespace ENSEK.Test
{
    public class RepositoryTest
    {
        public Mock<IDbAccessLayer> mock = new Mock<IDbAccessLayer>();
        private readonly Mock<ILogger<Repository>> _logger = new Mock<ILogger<Repository>>();


        [Fact]
        public void TestValidAccountAndLatestReadingInsertion()
        {
            var stream = File.OpenRead(@"./testcsv/Meter_Reading_1.csv");
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"./testcsv/Meter_Reading_1.csv"));
            mock.Setup(p => p.ValidAccountID(2344)).Returns(true);
            mock.Setup(p => p.GetLatestMeterReadingTime(2344)).Returns("");
            mock.Setup(p => p.SaveMeterReading(It.IsAny<MeterReading>())).Returns(true);

            IRepository repository = new Repository(mock.Object, _logger.Object);

            ResultResponse result = repository.MeterReadingUpload(file);
            Assert.Equal(2, result.PassRecords);
            Assert.Equal(6, result.FailRecords);
        }

        [Fact]
        public void TestInvalidAccountInsertion()
        {
            var stream = File.OpenRead(@"./testcsv/Meter_Reading_1.csv");
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"./testcsv/Meter_Reading_1.csv"));
            mock.Setup(p => p.ValidAccountID(2344)).Returns(false);

            IRepository repository = new Repository(mock.Object, _logger.Object);

            ResultResponse result = repository.MeterReadingUpload(file);
            Assert.Equal(0, result.PassRecords);
            Assert.Equal(8, result.FailRecords);
        }

        [Fact]
        public void TestOldRecordInsertion()
        {
            var stream = File.OpenRead(@"./testcsv/Meter_Reading_1.csv");
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"./testcsv/Meter_Reading_1.csv"));
            mock.Setup(p => p.ValidAccountID(2344)).Returns(true);
            mock.Setup(x => x.GetLatestMeterReadingTime(2344)).Returns("25/04/2019 09:24");
            mock.Setup(p => p.SaveMeterReading(It.IsAny<MeterReading>())).Returns(true);

            IRepository repository = new Repository(mock.Object, _logger.Object);

            ResultResponse result = repository.MeterReadingUpload(file);
            Assert.Equal(0, result.PassRecords);
            Assert.Equal(8, result.FailRecords);
        }

        [Fact]
        public void TestForEmptyFile()
        {
            var stream = File.OpenRead(@"./testcsv/Meter_Reading_2.csv");
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"./testcsv/Meter_Reading_2.csv"));
            mock.Setup(p => p.ValidAccountID(2344)).Returns(true);
            mock.Setup(x => x.GetLatestMeterReadingTime(2344)).Returns("");
            mock.Setup(p => p.SaveMeterReading(It.IsAny<MeterReading>())).Returns(true);

            IRepository repository = new Repository(mock.Object, _logger.Object);

            ResultResponse result = repository.MeterReadingUpload(file);
            Assert.Equal(0, result.PassRecords);
            Assert.Equal(0, result.FailRecords);
        }

        [Fact]
        public void TestForActualCsvFile()
        {
            var stream = File.OpenRead(@"./testcsv/Meter_Reading_3.csv");
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(@"./testcsv/Meter_Reading_3.csv"));
            mock.Setup(p => p.ValidAccountID(It.IsIn<int>(2344, 2233, 8766, 2345, 2346, 2347, 2348, 2349, 2350, 2351, 2352, 2353, 2355, 2356, 6776, 4534, 1234, 1239, 1240, 1241, 1242, 1243, 1244, 1245, 1246, 1247, 1248))).Returns(true);
            mock.Setup(x => x.GetLatestMeterReadingTime(2344)).Returns("");
            mock.Setup(p => p.SaveMeterReading(It.IsAny<MeterReading>())).Returns(true);

            IRepository repository = new Repository(mock.Object, _logger.Object);

            ResultResponse result = repository.MeterReadingUpload(file);
            Assert.Equal(25, result.PassRecords);
            Assert.Equal(10, result.FailRecords);
        }
    }
}
