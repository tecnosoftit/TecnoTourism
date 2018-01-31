using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using ViewModel.General;

namespace WebApi
{
    public class EventHub : Hub
    {
        public async Task Subscribe(string channel)
        {
            await Groups.Add(Context.ConnectionId, channel);
            var ev = new ChannelEvent
            {
                ChannelName = "Admomar",
                Name = "user.subscribed",
                Data = new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
            };
            await Publish(ev);
        }

        public async Task Unsubscribe(string channel)
        {
            await Groups.Remove(Context.ConnectionId, channel);
            var ev = new ChannelEvent
            {
                ChannelName = "Admomar",
                Name = "user.unsubscribed",
                Data = new
                {
                    Context.ConnectionId,
                    ChannelName = channel
                }
            };
            await Publish(ev);
        }


        public Task Publish(ChannelEvent channelEvent)
        {
            Clients.Group(channelEvent.ChannelName).OnEvent(channelEvent.ChannelName, channelEvent);
            if (channelEvent.ChannelName != "Admomar")
            {
                Clients.Group("Admomar").OnEvent("Admomar", channelEvent);
            }
            return Task.FromResult(0);
        }


        public override Task OnConnected()
        {
            var ev = new ChannelEvent
            {
                ChannelName = "Admomar",
                Name = "user.connected",
                Data = new
                {
                    Context.ConnectionId,
                }
            };
            Publish(ev);
            return base.OnConnected();
        }


        public override Task OnDisconnected(bool stopCalled)
        {
            var ev = new ChannelEvent
            {
                ChannelName = "Admomar",
                Name = "user.disconnected",
                Data = new
                {
                    Context.ConnectionId,
                }
            };
            Publish(ev);
            return base.OnDisconnected(stopCalled);
        }
    }
}