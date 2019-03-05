using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace NeonLogger
{
    public class NeonLogger
    {
        public void Log(string message)
        {
            if (_dict.ContainsKey(message))
            {
                _dict[message]++;
            }
            else
            {
                _dict[message] = 1;
            }

            lock (_lock)
            {
                File.AppendAllText(_filePath, message);
            }
        }

        public string[] PopularMessages()
        {
            return _dict.OrderByDescending(x => x.Value)
                .Take(10)
                .Select(x => x.Key)
                .ToArray();
        }

        public void LogDeffered(string message)
        {
            if (_queue.Count > 10000)
            {
                return;
            }

            _queue.Enqueue(message);
        }

        public void Run()
        {
            while (true)
            {
                while (_queue.TryDequeue(out var message))
                {
                    Log(message);
                }

                Thread.SpinWait(1);
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