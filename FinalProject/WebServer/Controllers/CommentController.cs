using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebServer.App_Data;
using WebServer.App_Data.Models;
using WebServer.App_Data.Models.Enums;

namespace WebServer.Controllers
{
    public class CommentController : ApiController
    {
        [HttpPost]
        [ActionName("AddVote")]
        public IHttpActionResult Vote([FromBody] VoteCommand vote)
        {
            using (var session = DBHelper.OpenSession())
            {
                Guid commentId;

                if (!Guid.TryParse(vote.Id, out commentId))
                {
                    return NotFound();
                }

                var comment = session.Get<Comment>(commentId);

                if (comment != null)
                {
                    comment.AddVote(new Vote(vote.Liked));
                    return Ok();
                }

                return NotFound();
            }
        }
    }

    public class VoteCommand {
        public string Id { get; set; }
        public bool Liked { get; set; }
    }
}
