using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;

namespace Async_Networking
{
    [TestClass]
    public class Test_Download
    {
        string url = "http://deelay.me/5000/http://delsink.com";

        [TestMethod]
        public void Test_Download_DelsinkCOM() // Synchronous method
        {
            var httpRequestInfo  = HttpWebRequest.CreateHttp( url );
            var httpResponseInfo = httpRequestInfo.GetResponse() as HttpWebResponse; // initial round trip to server

            var responseStream = httpResponseInfo.GetResponseStream(); // Downloading the page contents
            using ( var sr = new StreamReader(responseStream) )
            {
                var webPage = sr.ReadToEnd(); 
            }
        }

        [TestMethod]
        public void Test_Download_DelsinkCOM_Async() // Asynchronous method
        {
            var httpRequestInfo = HttpWebRequest.CreateHttp( url );

            var callBack    = new AsyncCallback( HttpResponseAvailable ); // this method will be call when the call is complete
            var asyncResult = httpRequestInfo.BeginGetResponse( callBack, null ); // initial round trip to server in async way

            asyncResult.AsyncWaitHandle.WaitOne(); // With this the method will wait for the callback to be done                                                                                         
        }

        private static void HttpResponseAvailable( IAsyncResult asyncResult )
        {
            // Error handling comes here


            var httpRequestInfo  = asyncResult as HttpWebRequest;
            var httpResponseInfo = httpRequestInfo.EndGetResponse(asyncResult) as HttpWebResponse;


            var responseStream = httpResponseInfo.GetResponseStream(); // Downloading the page contents
            using ( var sr = new StreamReader( responseStream ) )
            {
                var webPage = sr.ReadToEnd();
            }
        }
    }
}
