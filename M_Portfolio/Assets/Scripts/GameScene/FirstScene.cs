using UnityEngine;

using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace GameScene
{
    public class FirstScene : MonoBehaviour
    {
        [SerializeField] GameObject _objUserNameActiveText = null;
        [SerializeField] GameObject _objUserNameDeactiveText = null;
        [SerializeField] Button _btnLogin = null;

        [SerializeField] private Text _txtUserName = null;
        [SerializeField] private Text _txtLoginBtn = null;
        [SerializeField] private Text _txtLoginState = null;

        private const string _strLogin = "Login";
        private const string _strLogout = "Logout";
        private const string _strSuccess = "Success";
        private const string _strFail = "Fail";
        private const string _strDefault = "-";

        private void Awake()
		{
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();

            _btnLogin.onClick.AddListener(OnClickLogin);
            Refresh();
        }
        private void Refresh()
		{
            bool isAuthenticated = Social.localUser.authenticated;

            _txtLoginBtn.text = isAuthenticated ? _strLogout : _strLogin;
            _txtUserName.text = isAuthenticated ? Social.localUser.userName : string.Empty;
            _objUserNameActiveText.SetActive(isAuthenticated);
            _objUserNameDeactiveText.SetActive(!isAuthenticated);
        }
        #region Button Event
        private void OnClickLogin()
        {
            if (!Social.localUser.authenticated)
            {
                Social.localUser.Authenticate((bool bSuccess) =>
                {
                    _txtLoginState.text = bSuccess ? _strSuccess : _strFail;
                    Refresh();
                });
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SignOut();
                _txtLoginState.text = _strDefault;
                Refresh();
            }           
        }
		#endregion
	}
}

