using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Buzz
{
    public struct FreeSlot
    {
        public long Start;
        public long End;
        public FreeSlot(DateTime s, DateTime e)
        {
            Start = s.Ticks;
            End = e.Ticks;
        }
    };

    public static class ServiceInterface
    {
        //public List<FreeSlot> AvailableList { get; set; }

        /*
        txtOutput1.Text = "Create a User object and serialize it.";
        string json = WriteFromObject();
        txtOutput2.Text = json.ToString(); // Displays: {"Age":42,"Name":"Bob"}

        txtOutput3.Text = "Deserialize the data to a User object.";
        string jsonString = "{'Name':'Bill', 'Age':53}";
        User deserializedUser = ReadToObject(jsonString);
        txtOutput4.Text = deserializedUser.Name; // Displays: Bill
        txtOutput5.Text = deserializedUser.Age.ToString(); // Displays: 53
        */

        private static TimeSpan MIN_LENGTH = new TimeSpan(0, 30, 0); // 30 min in seconds
        private const string HOST_NAME = @"http://coffee.smartboyssite.net/";

        private static string BuildJson(List<FreeSlot> FreeList)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(FreeList.GetType());

            ser.WriteObject(ms, FreeList);
            byte[] json = ms.ToArray();
            ms.Close();

            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
        
        public static void SendFreeList(string MyID, string TheirID, List<FreeSlot> FreeList)
        {
            string json = BuildJson(FreeList);
            test(MyID, TheirID, json);
        }

        public static void test(string MyID, string TheirID, string json)
        {
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)  WebRequest.Create(HOST_NAME);

            // Set the Method property of the request to POST.
            request.Method = "POST";

            StringBuilder sb = new StringBuilder();
            sb.Append("MyId=");
            sb.Append(MyID);
            sb.Append("&TheirId=");
            sb.Append(TheirID);
            sb.Append("&MinLength=");
            sb.Append(MIN_LENGTH.Ticks.ToString());
            sb.Append("&MyFreeTime=");
            sb.Append(json);
            
            // Create POST data and convert it to a byte array.
            byte[] byteArray = Encoding.UTF8.GetBytes(sb.ToString());

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";

            MessageBox.Show(sb.ToString());
            
            // Set the ContentLength property of the WebRequest.
            //request.ContentLength = byteArray.Length;
            
            // Get the request stream.
            //using (Stream dataStream = request.GetRequestStream())
            {
                //dataStream.Write(byteArray, 0, byteArray.Length);
            }

            test();
        }

        static void test()
        {
            string outputBlock = "";

            try
            {      
                System.Uri uri = new Uri("http://www.contoso.com");

                // Create a HttpWebrequest object to the desired URL.
                HttpWebRequest myHttpWebRequest1= (HttpWebRequest)WebRequest.Create(uri);

                // Create an instance of the RequestState and assign the previous myHttpWebRequest1
                // object to it's request field.  
                RequestState myRequestState = new RequestState();  
                myRequestState.request = myHttpWebRequest1;

                // Start the asynchronous request.
                IAsyncResult result=
                (IAsyncResult) myHttpWebRequest1.BeginGetResponse(new AsyncCallback(RespCallback),myRequestState);
            }
            catch(WebException e)
            {
                outputBlock += "\nException raised!\n";
                outputBlock += "Message: ";
                outputBlock += e.Message;
                outputBlock += "\nStatus: ";
                outputBlock += e.Status;
                outputBlock += "\n";
            }
            catch(Exception e)
            {
                outputBlock += "\nException raised!\n";
                outputBlock += "\nMessage: ";
                outputBlock += e.Message;
                outputBlock += "\n";
            }

            if (outputBlock != "")
            {
                MessageBox.Show("oh shit!\n\n" + outputBlock);
            }
        }

        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState myRequestState=(RequestState) asynchronousResult.AsyncState;
                HttpWebRequest  myHttpWebRequest2=myRequestState.request;
                myRequestState.response = (HttpWebResponse) myHttpWebRequest2.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = myRequestState.response.GetResponseStream();
                myRequestState.streamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                //IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
            }
            catch (WebException e)
            {
                // Need to handle the exception
            }
        }

        /*
        // Create a User object and serialize it to a JSON stream.
        public static string WriteFromObject()
        {
            //Create User object.
            User user = new User("Bob", 42);

            //Create a stream to serialize the object to.
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(User));
            ser.WriteObject(ms, user);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
*/
    }

    public class RequestState
    {
        // This class stores the State of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder requestData;
        public byte[] BufferRead;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }
}
