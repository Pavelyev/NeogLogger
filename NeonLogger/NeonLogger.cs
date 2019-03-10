using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;

namespace NeonLogger
{
    public class NeonLogger
    {
        public void Log(string message)
        {
            _dict.AddOrUpdate(message, 1, (k, v) => v + 1);
            RealLog(message, true);
        }

        private void RealLog(string message, bool withLock)
        {
            if (withLock)
            {
                lock (_lock)
                {
                    File.AppendAllText(_filePath, message + "\n");
                }
            }
            else
            {
                File.AppendAllText(_filePath, message + "\n");
            }
        }

        public string[] PopularMessages()
        {
            return _dict.OrderByDescending(x => x.Value)
                .Take(10)
                .Select(x => x.Key)
                .ToArray();
        }

        public void LogDeferred(string message)
        {
            if (_queue.Count > 10000)
            {
                return;
            }

            _queue.Enqueue(message);
        }

        public void Flush()
        {
            lock (_lock)
            {
                while (_queue.TryDequeue(out var message))
                {
                    RealLog(message, withLock: false);
                }
            }
        }

        private readonly ConcurrentDictionary<string, int> _dict = new ConcurrentDictionary<string, int>();
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private readonly Object _lock = new Object();
        private readonly string _filePath;

        public NeonLogger(string filePath)
        {
            _filePath = filePath;
        }
    }
}