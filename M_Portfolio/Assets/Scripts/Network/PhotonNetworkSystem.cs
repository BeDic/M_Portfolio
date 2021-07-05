using Photon.Pun;
using Photon.Realtime;

namespace Network
{
    public class PhotonNetworkSystem : MonoBehaviourPunCallbacks
    {
        string _networkState;
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }
		public override void OnConnectedToMaster()
		{
			base.OnConnectedToMaster();
            PhotonNetwork.JoinLobby();
		}
		public override void OnJoinedLobby()
		{
			base.OnJoinedLobby();
			PhotonNetwork.JoinOrCreateRoom("room", new RoomOptions { MaxPlayers = 4 }, null);
		}
		void Update()
		{
			string currNetworkState = PhotonNetwork.NetworkClientState.ToString();
			if(_networkState != currNetworkState)
			{
				_networkState = currNetworkState;
				print(_networkState);
			}
		}
	}
}
