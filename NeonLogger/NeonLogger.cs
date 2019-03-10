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
            RealLog(message, true);
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

        public string[] PopularMessages()
        {
            return _dict.OrderByDescending(x => x.Value)
                .Take(10)
                .Select(x => x.Key)
                .ToArray();
        }

        private void RealLog(string message, bool withLock)
        {
            _dict.AddOrUpdate(message, 1, (k, v) => v + 1);
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

        private readonly ConcurrentDictionary<string, int> _dict = new ConcurrentDictionary<string, int>();
        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private readonly Object _lock = new Object();
        private readonly string _filePath;

        public NeonLogger(string filePath)
        {
            if (String.IsNullOrEmpty(filePath))
                throw new ArgumentException("prodive filePath", nameof(filePath));

            _filePath = filePath;

            var directoryName = Path.GetDirectoryName(filePath);

            if (String.IsNullOrEmpty(directoryName))
                return;

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
        }
    }
}