using ChatUlaR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatUlaR.Controllers
{
    public class ChatController : ApiController
    {
        private ChatModelsContext _ctx;
        public ChatController()
        {
            this._ctx = new ChatModelsContext();
        }

        public object Get() //IEnumerable<Room>
        {
            var TheRooms = this._ctx.Rooms.Select(c => new { c.RoomName, c.Id }).ToList();
            return TheRooms;
        }

        //// GET api/<controller>
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}