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
        // Ticks are in 100 nano second increments, scale to seconds
        public const long SCALE = (1000 * 1000 * 100);
        public long Start;
        public long End;
        public FreeSlot(DateTime s, DateTime e)
        {
            Start = s.Ticks / SCALE;
            End = e.Ticks / SCALE;
        }
        public FreeSlot(Slot s)
        {
            Start = s.Start.Ticks / SCALE;
            End = s.End.Ticks / SCALE;
        }
    };

    public static class ServiceInterface
    {
        private static TimeSpan MIN_LENGTH = new TimeSpan(0, 30, 0); // 30 min in seconds
        private const string HOST_NAME = @"http://coffee.smartboyssite.net/";
        const int BUFFER_SIZE = 1024;

        private static string ConstructJson(List<FreeSlot> FreeList)
        {
            MemoryStream ms = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(FreeList.GetType());

            ser.WriteObject(ms, FreeList);
            byte[] json = ms.ToArray();
            ms.Close();

            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

        private static List<FreeSlot> DeconstructJson(Stream stream)
        {
            List<FreeSlot> list;
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<FreeSlot>));
            list = (List<FreeSlot>) ser.ReadObject(stream);

            return list;
        }

        public static void CheckForResults(string MyID, string TheirID)
        {
            StringBuilder sb = new StringBuilder(HOST_NAME);
            sb.Append("result.php?MyID=");
            sb.Append(MyID);
            sb.Append("&TheirID=");
            sb.Append(TheirID);

            Uri uri = new Uri(sb.ToString());
            MessageBox.Show(uri.ToString());

            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);

            // Set the Method property of the request to POST.
            request.Method = "POST";

            RequestState rs = new RequestState();
            rs.request = request;

            // Start the asynchronous request.
            IAsyncResult result= (IAsyncResult) request.BeginGetResponse(new AsyncCallback(RespCallback), rs);
        }

        public static void SendFreeList(string MyID, string TheirID, List<FreeSlot> FreeList)
        {
            string json = ConstructJson(FreeList);

            StringBuilder sb = new StringBuilder(HOST_NAME);
            sb.Append("init.php?MyID=");
            sb.Append(MyID);
            sb.Append("&TheirID=");
            sb.Append(TheirID);
            sb.Append("&MinLength=");
            sb.Append((MIN_LENGTH.Ticks / FreeSlot.SCALE).ToString());
            sb.Append("&MyFreeTime=");
            sb.Append(json);

            Uri uri = new Uri(sb.ToString());
            System.Diagnostics.Debug.WriteLine("URI: {0}", uri.ToString());

            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(uri);

            // Set the Method property of the request to POST.
            request.Method = "POST";

            RequestState rs = new RequestState();
            rs.request = request;

            // Start the asynchronous request.
            IAsyncResult result= (IAsyncResult) request.BeginGetResponse(new AsyncCallback(RespCallback), rs);
        }

        private static void RespCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest2 = myRequestState.request;
                myRequestState.response = (HttpWebResponse)myHttpWebRequest2.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = myRequestState.response.GetResponseStream();
                myRequestState.streamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
            }
            catch (WebException)
            {
                // Need to handle the exception
            }
        }

        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                RequestState myRequestState = (RequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.streamResponse;
                int read = responseStream.EndRead(asyncResult);

                // Read the HTML page and then do something with it
                if (read > 0)
                {
                    myRequestState.requestData.Append(Encoding.UTF8.GetString(myRequestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                }
                else
                {
                    List<FreeSlot> MatchedList = null;

                    if (myRequestState.requestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = myRequestState.requestData.ToString();

                        if (stringContent != "false")
                        {
                            // We got data!!
                            // Need to polulate the list....
                            MatchedList = DeconstructJson(myRequestState.streamResponse);
                        }
                    }
                    responseStream.Close();

                    ResponseCallback(MatchedList);
                }
            }
            catch (WebException)
            {
                // Need to handle the exception
            }
        }

        public delegate void ResponseCallbackDelegate(List<FreeSlot> MatchedList);
        public static event ResponseCallbackDelegate ResponseCallback;
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
        public Decoder StreamDecode = Encoding.UTF8.GetDecoder();

        public RequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            requestData = new StringBuilder("");
            request = null;
            streamResponse = null;
        }
    }
}
