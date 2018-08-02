using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.WebService.Controllers
{
    public class GroupsController : ApiController
    {
        public GroupsController(
            IPasswdProvider provider,
            ILogger logger)
        {
            _provider = provider;
            _logger = logger;
        }

        [HttpGet]
        [ResponseType(typeof(List<Group>))]
        public IHttpActionResult Search([FromUri] GroupSearchRequest searchRequest)
        {
            Stopwatch stopwatch = start();
            Exception exception = null;
            List<Group> users = null;

            try
            {
                if (null == searchRequest ||
                    false == searchRequest.IsAnythingSet)
                {
                    users = _provider.GetAllGroups();
                }
                else
                {
                    users = _provider.Search(searchRequest);
                }
            }
            catch (HardFailureException hardFailure)
            {
                exception = hardFailure;
                _logger.Write(
                    hardFailure.Message,
                    LogEntrySeverityEnum.Error,
                    hardFailure);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.Write(
                    "An unknown error occurred. ",
                    LogEntrySeverityEnum.Error,
                    ex);
            }

            stop(stopwatch, "Group.Search");

            if (null == exception &&
                null == users)
            {
                return InternalServerError();
            }
            else if (null == exception)
            {
                return Ok(users);
            }
            else
            {
                return InternalServerError(exception);
            }
        }

        [HttpGet]
        [ResponseType(typeof(List<Group>))]
        [Route("api/groups/query")]
        public IHttpActionResult Query([FromUri] GroupSearchRequest searchRequest)
        {
            Stopwatch stopwatch = start();
            Exception exception = null;
            List<Group> users = null;

            try
            {
                if (null == searchRequest ||
                    false == searchRequest.IsAnythingSet)
                {
                    users = _provider.GetAllGroups();
                }
                else
                {
                    users = _provider.Search(searchRequest);
                }
            }
            catch (HardFailureException hardFailure)
            {
                exception = hardFailure;
                _logger.Write(
                    hardFailure.Message,
                    LogEntrySeverityEnum.Error,
                    hardFailure);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.Write(
                    "An unknown error occurred. ",
                    LogEntrySeverityEnum.Error,
                    ex);
            }

            stop(stopwatch, "Group.Query");

            if (null == exception &&
                null == users)
            {
                return InternalServerError();
            }
            else if (null == exception)
            {
                return Ok(users);
            }
            else
            {
                return InternalServerError(exception);
            }
        }

        [HttpGet]
        [ResponseType(typeof(Group))]
        [Route("api/groups/{gid}")]
        public IHttpActionResult Search(string gid)
        {
            Stopwatch stopwatch = start();
            Exception exception = null;
            List<Group> users = null;

            try
            {
                GroupSearchRequest searchRequest =
                    new GroupSearchRequest();
                searchRequest.GID = gid;

                users = _provider.Search(searchRequest);
            }
            catch (HardFailureException hardFailure)
            {
                exception = hardFailure;
                _logger.Write(
                    hardFailure.Message,
                    LogEntrySeverityEnum.Error,
                    hardFailure);
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.Write(
                    "An unknown error occurred. ",
                    LogEntrySeverityEnum.Error,
                    ex);
            }

            stop(stopwatch, "Group.Search");

            if (null == exception &&
                null == users)
            {
                return InternalServerError();
            }
            else if (null == exception)
            {
                if (users.Count != 1)
                {
                    return NotFound();
                }
                return Ok(users[0]);
            }
            else
            {
                return InternalServerError(exception);
            }
        }

        private IPasswdProvider _provider;
        private ILogger _logger;

        private Stopwatch start()
        {
            return Stopwatch.StartNew();
        }

        private void stop(Stopwatch stopwatch, string methodName)
        {
            stopwatch.Stop();

            Diagnostics.DiagnosticsLogger.LogCall(stopwatch.ElapsedMilliseconds);

            _logger.Write(methodName + ":" + stopwatch.ElapsedMilliseconds, LogEntrySeverityEnum.Debug);
        }
    }
}
