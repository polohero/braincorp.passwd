using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

using BrainCorp.Passwd.Entities;
using BrainCorp.Passwd.Business;
using BrainCorp.Passwd.Common.Logging;
using BrainCorp.Passwd.Common.Exceptions;

namespace BrainCorp.Passwd.WebService.Controllers
{
    public class UsersController : ApiController
    {
        public UsersController(
            IPasswdProvider provider,
            ILogger logger)
        {
            _provider = provider;
            _logger = logger;
        }

        [HttpGet]
        [ResponseType(typeof(List<User>))]
        public IHttpActionResult Search([FromUri] UserSearchRequest searchRequest)
        {
            Stopwatch stopwatch = start();

            Exception exception = null;
            List<User> users = null;

            try
            {
                if( null == searchRequest ||
                    false == searchRequest.IsAnythingSet)
                {
                    users = _provider.GetAllUsers();
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

            stop(stopwatch, "Users.Search");

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
        [ResponseType(typeof(List<User>))]
        [Route("api/users/query")]
        public IHttpActionResult Query([FromUri] UserSearchRequest searchRequest)
        {
            Stopwatch stopwatch = start();
            Exception exception = null;
            List<User> users = null;

            try
            {
                if (null == searchRequest ||
                    false == searchRequest.IsAnythingSet)
                {
                    users = _provider.GetAllUsers();
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

            stop(stopwatch, "Users.Query");

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
        [ResponseType(typeof(User))]
        [Route("api/users/{uid}")]
        public IHttpActionResult Search(string uid)
        {
            Stopwatch stopwatch = start();

            Exception exception = null;
            List<User> users = null;

            try
            {
                UserSearchRequest searchRequest =
                    new UserSearchRequest();
                searchRequest.UID = uid;

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

            stop(stopwatch, "Users.Search(string uid)");

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

        [HttpGet]
        [ResponseType(typeof(List<Groups>))]
        [Route("api/users/{uid}/groups")]
        public IHttpActionResult GetGroups(string uid)
        {
            Stopwatch stopwatch = start();
            Exception exception = null;
            List<Group> groups = null;

            try
            {
                UserSearchRequest searchRequest =
                    new UserSearchRequest();
                searchRequest.UID = uid;

                groups = _provider.GetGroups(searchRequest);
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

            stop(stopwatch, "Users.GetGroups");

            if (null == exception &&
                null == groups)
            {
                return InternalServerError();
            }
            else if (null == exception)
            {
                return Ok(groups);
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
