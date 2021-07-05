using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Network
{
	public class SocketNetworkSystem : MonoBehaviour
	{
		public void Start()
		{
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5959);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(endpoint);

			byte[] data = new byte[1024];
			socket.Receive(data);
			string buffer;// = Encoding.Default.GetString(data);

			buffer = "Receive Success";
			data = Encoding.Default.GetBytes(buffer);
			socket.Send(data);
			socket.Close();
		}		
	}
}