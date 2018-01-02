using System;
using System.Collections.Generic;
using Grapevine.Interfaces.Server;
using Grapevine.Shared;
using JARVIS.Shared;

namespace JARVIS.Core.Protocols.OAuth2
{
    public class OAuth2Provider
    {
        string _code = string.Empty;
        public string Token { get; private set; }
        string _refreshToken = string.Empty;
        int _expiresInSeconds;
        string _state;
        DateTime _expiresOn;

        string _provider = string.Empty;
        string _clientID = string.Empty;
        string _clientSecret = string.Empty;
        string _clientEncoded = string.Empty;
        string _scope = string.Empty;
        string _codeEndpoint = string.Empty;
        string _tokenEndpoint = string.Empty;
        string _refreshEndpoint = string.Empty;

        string _jarvisScope = string.Empty;

        public Action OnComplete;


        public bool Authenticated
        {
            get; private set;
        }

        public bool IsValid()
        {
            if (!Authenticated) return false;

            // Check/make for token refresh
            if ( DateTime.Now >= _expiresOn)
            {
                GetRefreshToken();
            }

            if (!Authenticated) return false;

            return true;
        }

        public OAuth2Provider()
        {
            Token = string.Empty;
        }
        public OAuth2Provider(string providerName, string clientID, string clientSecret, 
                              string scope, string codeEndpoint, string tokenEndpoint, 
                              string refreshEndpoint,
                              string jarvisScope = "default")
        {

            Token = string.Empty;

            _provider = providerName;
            _clientID = clientID;
            _clientSecret = clientSecret;
            _scope = scope;
            _codeEndpoint = codeEndpoint;
            _tokenEndpoint = tokenEndpoint;
            _refreshEndpoint = refreshEndpoint;
            _jarvisScope = jarvisScope;

            _clientEncoded = (_clientID + ":" + _clientSecret).Base64Encode();


        }

       

        public void Reset()
        {
            Token = string.Empty;
            Authenticated = false;
        }

        public void Login()
        {
            Authenticated = false;

            // Clear out other data
            Token = string.Empty;
            _code = string.Empty;
            _refreshToken = string.Empty;
            _expiresInSeconds = 0;

            Log.Message(_provider, "Initiating authorization process ...");

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            // Create random key that is used later to make sure this is the one that processes it
            _state = GenerateState();

            // These will be split and used in the function itself
            parameters.Add("endpoint", _codeEndpoint);
            parameters.Add("title", _provider + " Authentication");
            parameters.Add("message", "JARVIS needs to you to authenticate with your " + _provider + " account for it to be able to poll data.");

            // Data passed to each call (with the execption of the token)
            parameters.Add("client_id", _clientID);
            parameters.Add("client_secret", _clientSecret);

            // Ask for a lot of perms
            parameters.Add("scope", _scope);
            parameters.Add("state", _state);
            parameters.Add("redirect_uri", "http://" + Server.Config.Host + ":" + Server.Config.WebPort + "/callback/");

            // Add to listeners
            Server.Web.CallbackListeners.Add(_state, this);

            // Get socket service
            Services.Socket.SocketService socket = (Services.Socket.SocketService)Server.Provider.GetService(typeof(Services.Socket.SocketService));
            socket.SendToAllSessions(Shared.Protocol.Instruction.OpCode.OAUTH_REQUEST, parameters, true, _jarvisScope);
        }

        string GenerateState()
        {
           return  _provider.ToLower() + "-" + Guid.NewGuid().ToString();
        }

        internal void Callback(IHttpRequest request)
        {
            string state = request.QueryString.GetValue("state", string.Empty);

            // Stage 1 
            if (_code == string.Empty)
            {
                _code = request.QueryString.GetValue("code", string.Empty);

                if (_code != string.Empty)
                {
                    GetToken();
                }
            }
        }

        void GetToken()
        {
            // Create our token request
            var tokenRequest = new Requests.TokenRequest
            {
                Code = _code,
                RedirectURI = "http://" + Server.Config.Host + ":" + Server.Config.WebPort + "/callback/",
                State = _state
            };

            // Add our authorization header
            tokenRequest.Headers.Add("Authorization", "Basic " + _clientEncoded);

            // Get Response
            var responseObject = tokenRequest.GetResponse(_tokenEndpoint);

            if (responseObject != null)
            {

                if (!string.IsNullOrEmpty(responseObject.ErrorCode) && responseObject.ErrorCode != "")
                {
                    Log.Error(_provider, "An error occured (" + responseObject.ErrorCode + ") while getting the token. " + responseObject.ErrorDescription);
                    Authenticated = false;
                }
                else
                {
                    Token = responseObject.AccessToken;
                    _refreshToken = responseObject.RefreshToken;
                    _expiresInSeconds = responseObject.ExpiresInSeconds;
                    _scope = responseObject.Scope;
                    _expiresOn = DateTime.Now.AddSeconds(_expiresInSeconds);

                    // Flag we are good!
                    Log.Message(_provider, "Authentication Successful. (" + Token + ")");
                    OnComplete?.Invoke();
                    Authenticated = true;
                }
            }
            else
            {
                Authenticated = false;
                Log.Error(_provider, _provider + " failed to get the token. NULL Response Object.");
            }

        }

        void GetRefreshToken()
        {
            if (!Authenticated) return;

            Log.Message(_provider, "Refreshing Token");

            var tokenRequest = new Requests.RefreshTokenRequest(_refreshToken, _state);

            // Add our authorization header
            tokenRequest.Headers.Add("Authorization", "Basic " + _clientEncoded);

            // Get Response
            var responseObject = tokenRequest.GetResponse(_refreshEndpoint);

            if (responseObject != null)
            {

                if (!string.IsNullOrEmpty(responseObject.ErrorCode) && responseObject.ErrorCode != "")
                {
                    Log.Error(_provider, "An error occured (" + responseObject.ErrorCode + ") while refreshing the token. " + responseObject.ErrorDescription);
                    Authenticated = false;
                }
                else
                {
                    Token = responseObject.AccessToken;
                    _expiresInSeconds = responseObject.ExpiresInSeconds;
                    _scope = responseObject.Scope;
                    _expiresOn = DateTime.Now.AddSeconds(_expiresInSeconds);

                    // Flag we are good!
                    Authenticated = true;
                }
            }
            else
            {
                Authenticated = false;
                Log.Error(_provider, _provider + " failed to refresh the token. NULL Response Object.");
            }

        }

    }
}
