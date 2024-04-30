using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoChatManager : MonoBehaviour, IChatClientListener
{
    #region Setup
    [SerializeField] GameObject JoinChatButton;
    ChatClient chatClient;
    bool IsConnected;
    [SerializeField] string userName;
    public void SubmitPublicChatOnClick()
    {
        if(PrivateReceiver != "")
        {
            chatClient.PublishMessage("RegionChannel", CurrentChat);
            Chat_Field.text = "";
            CurrentChat = "";
        }
    }

    public void TypeChatOnValueChanged(string ValueIn)
    {
        CurrentChat = ValueIn;
    }
    public void UserNameOnValueChanged(string ValueIn)
    {
        userName = ValueIn;
    }
    public void ChatConnectOnClick()
    {
        IsConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userName));
        Debug.LogError("Connecting...");
    }
    #endregion Setup

  //  #region General
    [SerializeField] GameObject Chat_Panel;
    string PrivateReceiver;
    string CurrentChat;
    [SerializeField] InputField Chat_Field;
    [SerializeField] Text Chat_Display;


    public void DebugReturn(DebugLevel level, string message)
    {
     //   throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
      //  throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        Debug.LogError("Connected");
    //    IsConnected = true;
        JoinChatButton.SetActive(false);
        chatClient.Subscribe(new string[] { "RegionChannel" });
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for(int i = 0; i< senders.Length;i++)
        {
            msgs  = string.Format("{0}: {1}", senders[i], messages[i]);
            Chat_Display.text += "\n" + msgs;
            Debug.Log(msgs);
        }
       // throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Chat_Panel.SetActive(true);
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
       // chatClient = new ChatClient(this);
       // chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userId));
    }

    // Update is called once per frame
    void Update()
    {
        if(IsConnected) 
        {
            chatClient.Service();
        }
        if (Chat_Field.text != "" && Input.GetKey(KeyCode.Return))
        {
            SubmitPublicChatOnClick();
        }
    }
}  