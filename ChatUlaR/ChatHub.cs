using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ChatUlaR.Models;

namespace ChatUlaR
{
    public class ChatHub : Hub
    {

        public void JoinRoom(string roomname, string oldroomname)
        {

            var ctx = new ChatModelsContext();
            bool hasRoom = ctx.Rooms.Any(r => r.RoomName == roomname);

            //ctx.Database.Delete();

            if (!hasRoom)
            {
                var room = new Room { RoomName = roomname, DateCreated = DateTime.Now };
                ctx.Rooms.Add(room);
                ctx.SaveChanges();
                //Clients.All.sendNewRoom(room.Id, room.RoomName); //create ASYNCH rewrite the updating hub;
                Clients.Client(this.Context.ConnectionId).sendNewRoom(room.Id, room.RoomName);
                Clients.Others.addNewRoom(room.Id, room.RoomName);
            }

            var commentsNeeded = ctx.Comments
                .Where(r => r.ParentRoom.RoomName == roomname)
                .Select(m => new { m.DateCreated, m.UserName, m.Message }).ToList()
                .Select(j => new { DateCreated = j.DateCreated.ToString("g"), j.UserName, j.Message }).ToList(); //maybe rewrite this on the client side? might cause slowness.

            try
            {
                this.Groups.Remove(this.Context.ConnectionId, oldroomname); //removes from other groups when switching
            }
            catch
            {

            }

            this.Groups.Add(this.Context.ConnectionId, roomname); //works to switch users into certain groups
            Clients.Client(this.Context.ConnectionId).sendRoomMessages(commentsNeeded);
        }

        public void WriteMessage(Room theRoom, Comment theComment)
        {
            // Clients.All.addMessage(message.Msg, message.GroupName); //sends to all clients
            var ctx = new ChatModelsContext();
            var room = ctx.Rooms.FirstOrDefault(r => r.Id == theRoom.Id);

            if (room != null)
            {
                var newComment = new Comment { ParentRoom = room, UserName = theComment.UserName, Message = theComment.Message, DateCreated = DateTime.Now }; //saving comment to entity
                ctx.Comments.Add(newComment);
                ctx.SaveChanges();
            }

            Clients.Group(theRoom.RoomName).addMessage(DateTime.Now.ToString("g"), theComment.UserName, theComment.Message); //broadcasts message to specific group //message.UserName+": " +message.Msg SERVER WAY
        }
    }
}