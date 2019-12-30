using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Pool_lib;

namespace PoolTesting
{
    [TestClass]
    public class UnitTest
    {
        [TestInitialize]
        public void CleanTest() => PoolFactory.Clear();

        [TestMethod]
        public void TestCreation()
        {
            RegisterPoolAndAssertion<TypeOne>(1, 10);
            RegisterPoolAndAssertion<TypeTwo>(2, 10);
            RegisterPoolAndAssertion<TypeThree>(3, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestTryToCreateSamePool()
        {
            PoolFactory.RegisterPool_E<TypeOne>(10);
            PoolFactory.RegisterPool_E<TypeOne>(10);
        }

        [TestMethod]
        public void TestCreationAndGet()
        {
            RegisterPoolAndAssertion<TypeOne>(1, 10);

            TypeOne obj = PoolFactory.GetObject<TypeOne>();
            Assert.IsNotNull(obj);
        }


        [TestMethod]
        public void TestRegisterOverload()
        {
            int toCompare = 55;
            TypeTwo toClone = new TypeTwo(toCompare);
            PoolFactory.RegisterPool(toClone,2);

            TypeTwo obj = PoolFactory.GetObject<TypeTwo>();
            Assert.IsNotNull(obj);
            Assert.AreEqual(toCompare, obj.testParam);
        }

        [TestMethod]
        public void TestCreatWithNoObjectsAndGet()
        {
            RegisterPoolAndAssertion<TypeOne>(1, 0);
            TypeOne obj = PoolFactory.GetObject<TypeOne>();
            Assert.IsNotNull(obj);
            Assert.AreEqual(0, PoolFactory.GetPoolSize<TypeOne>());

            PoolFactory.Recycle(ref obj);
            Assert.AreEqual(1, PoolFactory.GetPoolSize<TypeOne>());
        }


        [TestMethod]
        public void TestGetTypeNotPresent()
        {
            RegisterPoolAndAssertion<TypeOne>(1);
            TypeTwo obj = PoolFactory.GetObject<TypeTwo>();
            Assert.IsNull(obj);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestGetTypeNotPresentThrowException()
        {
            RegisterPoolAndAssertion<TypeOne>(1);
            TypeTwo obj = PoolFactory.GetObject_E<TypeTwo>();
        }

        [TestMethod]
        public void TestRemovePresentObject()
        {
            RegisterPoolAndAssertion<TypeOne>(1);
            PoolFactory.RemovePool<TypeOne>();
            Assert.AreEqual(PoolFactory.ListOfPools.Count, 0);
        }

        [TestMethod]
        public void TestRemoveNotPresentObject()
        {
            PoolFactory.RemovePool<TypeOne>();
            Assert.AreEqual(PoolFactory.ListOfPools.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestRemoveNotPresentObjectThrowException()
        {
            PoolFactory.RemovePool_E<TypeOne>();
            Assert.AreEqual(PoolFactory.ListOfPools.Count, 0);
        }

        [TestMethod]
        public void TestRecycle()
        {
            RegisterPoolAndAssertion<TypeOne>(1);
            Assert.AreEqual(10, PoolFactory.GetPoolSize<TypeOne>());

            TypeOne obj = PoolFactory.GetObject<TypeOne>();
            Assert.AreEqual(9, PoolFactory.GetPoolSize<TypeOne>());

            PoolFactory.Recycle(ref obj);
            Assert.AreEqual(10, PoolFactory.GetPoolSize<TypeOne>());

        }

        [TestMethod]
        public void TestCompleteFlow()
        {
            PoolFactory.RegisterPool<TypeTwo>();

            // Specify a value to have some instances of objects according to your needs  
            PoolFactory.RegisterPool<TypeOne>(10);

            TypeOne obj_0 = PoolFactory.GetObject<TypeOne>();

            // Use methods with "_E" if you want throw Exception  
            TypeTwo obj_1 = PoolFactory.GetObject_E<TypeTwo>();

            PoolFactory.Recycle(ref obj_0);

        }

        private void RegisterPoolAndAssertion<T>(int instancesExpected, int objToInstantiate = 10) where T : class, new()
        {
            PoolFactory.RegisterPool<T>(objToInstantiate);
            Assert.AreEqual(PoolFactory.ListOfPools.Count, instancesExpected);
        }

        private class TypeOne { }

        private class TypeTwo {
            public int testParam = 10;

            public TypeTwo(int testParam)
            {
                this.testParam = testParam;
            }

            public TypeTwo()
            {

            }
        }

        private class TypeThree { }
    }

}

