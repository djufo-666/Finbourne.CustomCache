using Microsoft.VisualStudio.TestTools.UnitTesting;
using Finbourne.CustomCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finbourne.CustomCache.Tests
{
    [TestClass()]
    public class CacheServiceTests
    {
        const string defaultKey = "key1";
        const string defaultItem = "item to cache 1";
        const string defaultKey2 = "key2";
        const string defaultItem2 = "item to cache 2";

        [TestMethod()]
        public void Construction_Test()
        {
            // Arange
            const int capacity = 10;

            // Act
            CacheService sut = new CacheService(capacity);

            // Assert
            Assert.AreEqual(capacity, sut.Capacity);
            Assert.AreEqual(0, sut.Size);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exceptions.CacheServiceCapacityException))]
        public void Capacity_LessThanOne_ThrowsException()
        {
            // Arange

            // Act
            CacheService sut = new CacheService(0);

            // Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void Add_FirstItem_ShouldBeCached()
        {
            // Arange
            CacheService sut = new CacheService(10);

            // Act
            sut.Add(defaultKey, defaultItem);

            string item = sut.Get<string>(defaultKey);

            // Assert
            Assert.AreEqual(defaultItem, item);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exceptions.CacheServiceKeyExistsException))]
        public void Add_KeyAllreadyExists_ThrowsException()
        {
            // Arange
            CacheService sut = new CacheService(1);

            // Act
            sut.Add(defaultKey, defaultItem);
            sut.Add(defaultKey, defaultItem2);

            // Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void Add_CapacityOverload_ItemBeforeShouldBeRemoved()
        {
            // Arange
            CacheService sut = new CacheService(1);

            // Act
            sut.Add(defaultKey, defaultItem);
            sut.Add(defaultKey2, defaultItem2);

            string item1;
            bool item1Found = sut.TryGet<string>(defaultKey, out item1);
            string item2 = sut.Get<string>(defaultKey2);
            // Assert
            Assert.IsNull(item1);
            Assert.IsFalse(item1Found);
            Assert.AreEqual(defaultItem2, item2);
        }

        [TestMethod()]
        public void Add_CapacityOverload_MostRecentlyUsedItemWillStay()
        {
            // Arange
            CacheService sut = new CacheService(2);

            // Act
            sut.Add(defaultKey, defaultItem);
            sut.Add(defaultKey2, defaultItem2);
            sut.Get<string>(defaultKey);
            sut.Add("key3", "item3");

            string item1 = sut.Get<string>(defaultKey);
            string item2;
            bool item2Found = sut.TryGet<string>(defaultKey2, out item2);
            // Assert
            Assert.AreEqual(defaultItem, item1);
            Assert.IsNull(item2);
            Assert.IsFalse(item2Found);
        }


        [TestMethod()]
        [ExpectedException(typeof(Exceptions.CacheServiceKeyNotFoundException))]
        public void Get_NotInCache_ThrowsException()
        {
            // Arange
            CacheService sut = new CacheService(1);

            // Act
            sut.Add(defaultKey, defaultItem);
            sut.Get<string>(defaultKey2);

            // Assert
            Assert.Fail();
        }

        [TestMethod()]
        public void TryGet_NotInCache_ReturnsFalse()
        {
            // Arange
            CacheService sut = new CacheService(1);

            // Act
            sut.Add(defaultKey, defaultItem);

            string item;
            bool exists = sut.TryGet<string>(defaultKey2, out item);

            // Assert
            Assert.IsNull(item);
            Assert.IsFalse(exists);
        }
        [TestMethod()]
        public void TryGet_InCache_ReturnsTrueAndItem()
        {
            // Arange
            CacheService sut = new CacheService(1);

            // Act
            sut.Add(defaultKey, defaultItem);

            string item;
            bool exists = sut.TryGet<string>(defaultKey, out item);

            // Assert
            Assert.AreEqual(defaultItem, item);
            Assert.IsTrue(exists);
        }


    }
}