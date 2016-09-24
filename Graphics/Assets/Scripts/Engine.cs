using Hive.IpcClient;
using UnityEngine;

namespace Assets
{
    /// <summary>
    /// Możesz używać tej klasy z dowolnego miejsca poprzez Assets.Engine.(...)
    /// </summary>
    public class Engine : MonoBehaviour
    {
        /// <summary>
        /// Obiekt zawierający silnik gry
        /// </summary>
        public static HiveClient Client { get; private set; }

        /// <summary>
        /// Metoda resetująca silnik - musisz ją wywołać przed pierwszym użyciem
        /// </summary>
        public static void Reset()
        {
            Stop();
            Debug.Log("Starting engine...");
            Debug.Log(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Debug.Log(System.Environment.CurrentDirectory);
            Client = new HiveClient(Debug.Log, 0);
            Debug.Log("Engine started.");
        }

        /// <summary>
        /// Metoda zatrzymująca silnik - nie musisz jej wywoływać, program sam to zrobi :)
        /// </summary>
        public static void Stop()
        {
            if (Client != null)
            {

                Debug.Log("Stopping engine...");
                Client.Dispose();
                Client = null;
                Debug.Log("Engine stopped.");
            }
        }

        void OnApplicationQuit()
        {
            Stop();
        }
    }
}
