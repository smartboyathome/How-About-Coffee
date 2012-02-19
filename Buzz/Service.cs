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
        public DateTime Start;
        public DateTime End;
        public FreeSlot(DateTime s, DateTime e)
        {
            Start = s;
            End = e;
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

        private static const int MIN_LENGTH = 30 * 60; // 30 min in seconds
        private static const string HOST_NAME = @"coffee.smartboyssite.net";

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
            string strJson = BuildJson(FreeList);
            MessageBox.Show(strJson);

            
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
}
