using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;

namespace Async_Databases
{
    [TestClass]
    public class Test_Databases
    {
        [TestMethod]
        public void Test_DB_Sync() // Synchronous method
        {
            string connString = "";

            string sqlSelect = "SELECT @@VERSION";

            using ( var sqlConnection = new SqlConnection( connString ) )
            {
                sqlConnection.Open();

                using ( var sqlCommand = new SqlCommand( sqlSelect, sqlConnection ) )
                {
                    using ( var reader = sqlCommand.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            var data = reader[0].ToString();
                        }
                    }
                }
            }
        }


        [TestMethod]
        public void Test_DB_ASync() // Asynchronous method
        {
            string connString = "";

            string sqlSelect = "SELECT @@VERSION";

            using ( var sqlConnection = new SqlConnection( connString ) )
            {
                sqlConnection.Open();

                var sqlCommand = new SqlCommand( sqlSelect, sqlConnection );

                var callBack = new AsyncCallback( DataAvailable );
                var asyncResult = sqlCommand.BeginExecuteReader( callBack, sqlCommand );
                

                asyncResult.AsyncWaitHandle.WaitOne();
            }
        }

        private static void DataAvailable( IAsyncResult asyncResult )
        {
            var sqlCommand = asyncResult.AsyncState as SqlCommand;
            using ( var reader = sqlCommand.EndExecuteReader( asyncResult ) )          
            {
                while ( reader.Read() )
                {
                    var data = reader[0].ToString();
                }
            }
        }
    }
}
