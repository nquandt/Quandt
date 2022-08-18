using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Quandt.Expressions.Javascript.Helpers
{
    public class DisposeHelper
    {
        private readonly bool _threadSafe;

        private int _disposed;

        public bool Disposed
        {
            get
            {
                int num = _threadSafe ? Interlocked.CompareExchange(ref _disposed, 1, 0) : _disposed;
                _disposed = 1;
                return num == 1;
            }
        }

        public DisposeHelper(bool threadSafe)
        {
            _threadSafe = threadSafe;
        }
    }
}
