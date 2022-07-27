using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Testing_Delegates_Framework
{
    [TestClass]
    public class Test_Delegates_Framework
    {
        // This scenario is not yet a real asynchronous way
        private void DoWork()
        {
            Debug.WriteLine("Testing delegates...");
            Debug.WriteLine("First thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
        }

        delegate void DoWork_Delegate();

        [TestMethod]
        public void Test01()
        {
            Debug.WriteLine("Second thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

            DoWork_Delegate doWork_Delegate = new DoWork_Delegate(DoWork);

            IAsyncResult asyncResult = doWork_Delegate.BeginInvoke(null, null);

            // Do some work

            doWork_Delegate.EndInvoke(asyncResult);

        }

        //-------------------------------------------------------
        // This scenario is a real asynchronous way
        private void DoWork_2()
        {
            Debug.WriteLine("Testing delegates...");
            Debug.WriteLine("First thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
        }

        delegate void DoWork_Delegate_2();

        [TestMethod]
        public void Test02()
        {
            Debug.WriteLine("Second thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

            DoWork_Delegate doWork_Delegate = new DoWork_Delegate(DoWork_2);

            // This will be the callback
            AsyncCallback callBack = new AsyncCallback(CallBackMethod);
            IAsyncResult asyncResult = doWork_Delegate.BeginInvoke(callBack, doWork_Delegate);

            // Do some work

            // This is a bad way to wait for the DoWork_2 method to finish before the Test02 exits
            //System.Threading.Thread.Sleep(4000);

            // Wait for a signal instead
            asyncResult.AsyncWaitHandle.WaitOne();
        }
        private static void CallBackMethod( IAsyncResult asyncResult )
        {
            // We are passing the the delegate method as a state of the asyncResult
            var doWork_Delegate = asyncResult.AsyncState as DoWork_Delegate;

            doWork_Delegate.EndInvoke(asyncResult); // This is where you use try/catch
        }
    }
}
