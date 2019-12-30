using System;
using System.Collections.Generic;

namespace Pool_lib
{
    public static class PoolFactory
    {
        private static Dictionary<Type, IPoolable> containers = new Dictionary<Type, IPoolable>();

        private static List<string> pools = new List<string>();
        private static IPoolable tempPoolable = new Pool<object>();

        public static List<string> ListOfPools
        {
            get
            {
                foreach (IPoolable item in containers.Values)
                {
                    if (!pools.Contains(item.ToString()))
                        pools.Add(item.ToString());
                }
                return pools;
            }
        }

        public static int GetPoolSize<T>() where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                return (tempPoolable as Pool<T>).Count;
            }

            Console.WriteLine("Pool factory don't contains any pool of this type");
            return 0;
        }

        public static int GetPoolSize_E<T>() where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                return (tempPoolable as Pool<T>).Count;
            }

            throw new Exception("Pool factory don't contains any pool of this type");
        }

        public static void RegisterPool<T>(int preObject = 0) where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                Console.WriteLine("Pool already registered");
                return;
            }

            containers.Add(typeof(T), new Pool<T>(preObject));
        }

        public static void RegisterPool<T>(T objectType,int preObject = 0) where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                Console.WriteLine("Pool already registered");
                return;
            }

            containers.Add(typeof(T), new Pool<T>(objectType, preObject));
        }

        public static void RegisterPool_E<T>(int preObject = 0) where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
                throw new Exception("Pool already registered");

            containers.Add(typeof(T), new Pool<T>(preObject));
        }

        public static T GetObject<T>() where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                return (tempPoolable as Pool<T>).GetObject();
            }

            Console.WriteLine("Pool don't contains any pool of this type");
            return null;
        }

        public static T GetObject_E<T>() where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                return (tempPoolable as Pool<T>).GetObject();
            }

            throw new Exception("Pool don't contains any pool of this type");
        }

        public static void RemovePool<T>() where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                pools.Remove(tempPoolable.ToString());
                containers.Remove(typeof(T));
            }
            else
                Console.WriteLine("Pool don't contains any pool of this type");
        }

        public static void RemovePool_E<T>() where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                pools.Remove(tempPoolable.ToString());
                containers.Remove(typeof(T));
            }
            else
                throw new Exception("Pool don't contains any pool of this type");
        }

        public static void Recycle<T>(ref T toRecycle) where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                (tempPoolable as Pool<T>).Recycle(toRecycle);
            }
            else
                Console.WriteLine("Pool don't contains any pool of this type");
        }

        public static void Recycle_E<T>(ref T toRecycle) where T : class, new()
        {
            if (containers.ContainsKey(typeof(T)))
            {
                containers.TryGetValue(typeof(T), out tempPoolable);
                (tempPoolable as Pool<T>).Recycle(toRecycle);
            }
            else
                throw new Exception("Pool don't contains any pool of this type");
        }

        public static void Clear()
        {
            containers.Clear();
            pools.Clear();
            tempPoolable = null;
        }

        private interface IPoolable { }

        private class Pool<T> : IPoolable where T : class, new()
        {
            private Queue<T> container = new Queue<T>();

            public int Count => container.Count;

            public Pool(int preObject = 0)
            {
                for (int i = 0; i < preObject; i++)
                    container.Enqueue(new T());
            }

            public Pool(T obj ,int preObject = 0)
            {
                for (int i = 0; i < preObject; i++)
                    container.Enqueue(obj);
            }

            public Pool() { }

            ~Pool() => container.Clear();

            public T GetObject()
            {
                if (container.Count > 0)
                    return container.Dequeue();

                return new T();
            }

            public void Recycle(T toRecycle) => container.Enqueue(toRecycle);

            public override string ToString()
            {
                return "Pool of: " + typeof(T);
            }

        }
    }
}

