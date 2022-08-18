using System;
using System.Collections.Generic;
using System.Text;

namespace Quandt.Expressions.Javascript.Helpers
{
    public class GenericDisposable : IDisposable
    {
        public GenericDisposable()
        {
        }

        public GenericDisposable(Action disposeAction)
          : this(disposeAction, false)
        { }

        public GenericDisposable(Action disposeAction, bool threadSafe)
        {
            this.DisposeAction = disposeAction;
            this.DisposeHelper = new DisposeHelper(threadSafe);
        }

        protected Action DisposeAction { get; set; }

        protected DisposeHelper DisposeHelper { get; set; }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.DisposeHelper == null || this.DisposeHelper.Disposed || this.DisposeAction == null)
                return;
            this.DisposeAction();
        }
    }
}
