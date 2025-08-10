using ExitGames.Client.Photon;
using GorillaInfoWatch.Extensions;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GorillaCustomSkin.Behaviours.Networking
{
    // https://github.com/developer9998/GorillaInfoWatch/blob/fbfcf043668cd4e90963836a5aecdbbbc56e8e6a/GorillaInfoWatch/Behaviours/Networking/NetworkHandler.cs
    public class NetworkManager : Singleton<NetworkManager>, IInRoomCallbacks
    {
        public Action<NetPlayer, Dictionary<string, object>> OnPlayerPropertyChanged;

        private readonly Dictionary<string, object> properties = [];
        private bool setProperties = false;
        private float properties_timer;

        public override void Initialize()
        {
            if (NetworkSystem.Instance is NetworkSystem netSys && netSys is NetworkSystemPUN)
            {
                SetProperty("Version", Constants.Version);

                PhotonNetwork.AddCallbackTarget(this);
                Application.quitting += () => PhotonNetwork.RemoveCallbackTarget(this);
                return;
            }

            enabled = false; // either no netsys or not in a pun environment - i doubt fusion will ever come
        }

        public void FixedUpdate()
        {
            properties_timer -= Time.deltaTime;

            if (setProperties && properties.Count > 0 && properties_timer <= 0)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new()
                {
                    { Constants.NetworkPropertyKey, new Dictionary<string, object>(properties) }
                });
                setProperties = false;
                properties_timer = Constants.NetworkRaiseInterval;
            }
        }

        public void SetProperty(string key, object value)
        {
            if (properties.ContainsKey(key)) properties[key] = value;
            else properties.Add(key, value);
            setProperties = true;
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            NetPlayer netPlayer = NetworkSystem.Instance.GetPlayer(targetPlayer.ActorNumber);

            if (!VRRigCache.Instance.TryGetVrrig(netPlayer, out RigContainer playerRig) || !playerRig.TryGetComponent(out NetworkedPlayer networkedPlayer))
                return;

            if (changedProps.TryGetValue(Constants.NetworkPropertyKey, out object propertiesObject) && propertiesObject is Dictionary<string, object> properties)
            {
                Plugin.Logger.LogMessage($"Recieved properties from {netPlayer.GetNameRef().SanitizeName()}");
                Plugin.Logger.LogInfo(string.Join(Environment.NewLine, properties.Select(prop => $"[{prop.Key}, {prop.Value}]")));

                if (netPlayer.IsLocal) return;

                if (!networkedPlayer.HasCustomSkin)
                {
                    networkedPlayer.HasCustomSkin = true;
                    Plugin.Logger.LogMessage("Player has GorillaCustomSkin");
                }

                OnPlayerPropertyChanged?.Invoke(netPlayer, properties);
            }
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {

        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {

        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {

        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {

        }
    }
}
