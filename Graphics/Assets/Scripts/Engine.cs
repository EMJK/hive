using System;
using Hive.IpcClient;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class Engine : MonoBehaviour
    {
        public static HiveClient Client;

        public static void Reset()
        {
            try
            {
                if (Client != null)
                {
                    Client.Dispose();
                }
                Client = new HiveClient();
                Debug.Log("It works!");
            }
            catch (Exception ex)
            {
                Debug.Log("Failed to load HiveClient: " + ex);
            }
        }

        void OnApplicationQuit()
        {
            Client.Dispose();
        }
    }
}