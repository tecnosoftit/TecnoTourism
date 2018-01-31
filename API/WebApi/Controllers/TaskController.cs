using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using ViewModel.General;

namespace WebApi.Controllers
{
    public class TaskController : ApiController
    {
        private IHubContext _context;
        private string _channel = "Admomar";

        public TaskController()
        {
            _context = GlobalHost.ConnectionManager.GetHubContext<EventHub>();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/tasks/long")]
        public IHttpActionResult GetLongTask()
        {
            double steps = 10;
            var eventName = "longTask.status";

            ExecuteTask(eventName, steps);

            return Ok("Long task complete");
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/tasks/short")]
        public IHttpActionResult GetShortTask()
        {
            double steps = 5;
            var eventName = "shortTask.status";
            ExecuteTask(eventName, steps);
            return Ok("Short task complete");
        }

        private void ExecuteTask(string eventName, double steps)
        {
            var status = new Status
            {
                State = "starting",
                PercentComplete = 0.0
            };

            PublishEvent(eventName, status);

            for (double i = 0; i < steps; i++)
            {
                status.State = "working";
                status.PercentComplete = (i / steps) * 100;
                PublishEvent(eventName, status);
                Thread.Sleep(500);
            }
            status.State = "complete";
            status.PercentComplete = 100;
            PublishEvent(eventName, status);
        }

        private void PublishEvent(string eventName, Status status)
        {
            _context.Clients.Group(_channel).OnEvent(eventName, status);
        }
    }
}
