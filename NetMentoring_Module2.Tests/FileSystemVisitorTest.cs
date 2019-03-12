using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace NetMentoring_Module2.Tests
{
    [TestClass]
    public class FileSystemVisitorTest
    {
        [TestMethod]
        public void FileSystemVisitor_GetFilesMethod_ReturnsCountPropertyAboveZero()
        {
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog");
            fileSystemVisitor.GetFiles();
            Assert.IsTrue(fileSystemVisitor.Count > 0);
        }

        [TestMethod]
        public void FileSystemVisitor_GetFilesMethodWithDelegate_ReturnsCountPropertyAboveZero()
        {
            Func<string, bool> sortingDelegate = (directory) =>
            {
                if (directory.Length > 45)
                    return false;
                else
                    return true;
            };
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog",sortingDelegate);
            fileSystemVisitor.GetFiles();
            Assert.IsTrue(fileSystemVisitor.Count > 0);
        }

        [TestMethod]
        public void FileSystemVisitor_GetFilesMethod_ReturnsResultPropertyLengthAboveZero()
        {
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog");
            fileSystemVisitor.GetFiles();
            Assert.IsTrue(fileSystemVisitor.Result.Count > 0);
        }

        [TestMethod]
        public void FileSystemVisitor_GetFilesMethodCalledTwice_ReturnsTheSameValuesOfCountProperty()
        {
            int firstCall = 0;
            int secontCall = 0;
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog");
            fileSystemVisitor.GetFiles();
            firstCall = fileSystemVisitor.Count;
            fileSystemVisitor.GetFiles();
            secontCall = fileSystemVisitor.Count;
            Assert.AreEqual(firstCall, secontCall);
        }
        [TestMethod]
        public void FileSystemVisitor_GetFilesMethodCalledTwice_ReturnsTheSameValuesOfResultPropertyLength()
        {
            int firstCall = 0;
            int secontCall = 0;
            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog");
            fileSystemVisitor.GetFiles();
            firstCall = fileSystemVisitor.Result.Count;
            fileSystemVisitor.GetFiles();
            secontCall = fileSystemVisitor.Result.Count;
            Assert.AreEqual(firstCall, secontCall);
        }
        [TestMethod]
        public void FileSystemVisitor_GetFilesMethodCalledWithDelegateAlwaysTrue_ReturnsResultPropertyLengthAboveZero()
        {
            var mock = new Mock<Func<string,bool>>();
            mock.Setup(del => del(It.IsAny<string>())).Returns(true);

            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog", mock.Object);

            fileSystemVisitor.GetFiles();

            Assert.IsTrue(fileSystemVisitor.Result.Count > 0);
        }

        [TestMethod]
        public void FileSystemVisitor_GetFilesMethodCalledWithDelegateAlwaysFalse_ReturnsResultPropertyLengthEquelsToZero()
        {
            var mock = new Mock<Func<string, bool>>();
            mock.Setup(del => del(It.IsAny<string>())).Returns(false);

            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog", mock.Object);

            fileSystemVisitor.GetFiles();

            Assert.IsTrue(fileSystemVisitor.Result.Count == 0);
        }

        [TestMethod]
        public void FileSystemVisitor_GetFilesMethodCalledWithDelegateAlwaysTrue_ResultDelegateCalledTimesAreEquelToCountProperty()
        {
            var mock = new Mock<Func<string, bool>>();
            mock.Setup(del => del(It.IsAny<string>())).Returns(true);

            FileSystemVisitor fileSystemVisitor = new FileSystemVisitor(@"D:\TestCatalog", mock.Object);

            fileSystemVisitor.GetFiles();

            mock.Verify(del => del(It.IsAny<string>()), Times.Exactly(fileSystemVisitor.Count));


        }
    }
}
