using System.Diagnostics;

namespace Testing_Delegates
{
    [TestClass]
    public class Test_Delegates_Core
    {
        

        private void DoWork()
        {
            Debug.WriteLine("Testing delegates...");
            Debug.WriteLine("First thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
        }

        delegate void DoWork_Delegate();

        //[TestMethod]
        //public void Test01()
        //{
        //    Debug.WriteLine("Second thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

        //    DoWork_Delegate doWork_Delegate = new DoWork_Delegate(DoWork);

        //    IAsyncResult asyncResult = doWork_Delegate.BeginInvoke(null, null);

        //    // Do some work

        //    doWork_Delegate.EndInvoke(asyncResult);

        //}

        // So it turns out that BeginInvoke and EndInvoke are not supported anymore on .NET Core, only on Framework
        // So the solution is the following code to this situation ->

        [TestMethod]
        public async Task Test01_CoreVersion()
        {
            Debug.WriteLine("Second thread ID: " + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());

            DoWork_Delegate doWork_Delegate = DoWork;

            // Schedule the work using a Task and 
            // del.Invoke instead of del.BeginInvoke.
            Console.WriteLine("Starting with Task.Run");
            var workTask = Task.Run( () => doWork_Delegate.Invoke() );

            // Optionally, we can specify a continuation delegate 
            // to execute when DoWork has finished.
            var followUpTask = workTask.ContinueWith( delegate { TaskCallback(); } );

            // This writes output to the console while DoWork is running in the background.
            Debug.WriteLine("Waiting on work...");

            // We await the task instead of calling EndInvoke.
            // Either workTask or followUpTask can be awaited depending on which
            // needs to be finished before proceeding. Both should eventually
            // be awaited so that exceptions that may have been thrown can be handled.
            await workTask;
            await followUpTask;

        }

        private void TaskCallback()
        {
            Debug.WriteLine("Doing some another work...");
        }
    }
}