using System;
using System.Net;
using System.Net.Sockets;
using VAICOM;
using VAICOM.Client;
using VAICOM.Products;
using VAICOM.Settings;
using Xunit;

public class SendNewMessageTests
{
    private class DummyDictation
    {
        public bool IsOn() => false;
    }

    private class DummyProxy
    {
        public DummyDictation Dictation { get; } = new DummyDictation();
        public void WriteToLog(string message, string color) { }
    }

    [Fact]
    public void SendNewMessage_AllowsNullRadioDelayAndTriggersUpdate()
    {
        // Arrange
        State.Proxy = new DummyProxy();
        State.SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        State.SendIpEndPoint = new IPEndPoint(IPAddress.Loopback, 12345);
        State.activeconfig = new Config { ForcedLanguage = 0 };
        State.currentlicense = string.Empty;
        State.currentradiodevicename = string.Empty;
        State.currentmodule = new DCSmodule { radiodelay = null };
        State.currentmessage = new DcsClient.Message.CommsMessage { command = 4000 };
        State.lastupdaterequesttimer = -1;

        // Act
        Exception ex = Record.Exception(() => DcsClient.Message.SendNewMessage());

        // Assert
        Assert.Null(ex);
        Assert.Equal(0, State.lastupdaterequesttimer);
    }
}
