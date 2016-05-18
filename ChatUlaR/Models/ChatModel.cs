using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ChatUlaR.Models
{
    public class ChatModelsContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MyMessage> Messages { get; set; }
    }
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string RoomName { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Room ParentRoom { get; set; }
    }

    public class MyMessage
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Msg { get; set; }
        public string GroupName { get; set; }
    }
}