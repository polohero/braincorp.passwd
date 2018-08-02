using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BrainCorp.Passwd.Common.Logging;

namespace BrainCorp.Passwd.Testing.Common
{
    public abstract class UnitTestBase
    {
        #region Public Properties

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        protected TestContext testContextInstance;

        public TestContextLogger TestLog { get; set; }

        #endregion

        #region Protected Methods

        protected void initialize()
        {
            TestLog = new TestContextLogger(TestContext);
            TestLog.SetLoggableSeverities(LogEntrySeverityEnum.Debug);
        }
        
        protected void evaluate(
            bool result,
            string name,
            string message,
            string current,
            string expected)
        {
            log(
                "Test:" + name + ":" +" Result: " + ( true == result ? "PASS" : "FAILURE" ),
                LogEntrySeverityEnum.Debug
                );

            Assert.IsTrue(
               result,
               message + ":" + Environment.NewLine + 
               "Current:" + current + Environment.NewLine + 
               "Expected:" + expected);
        }
        protected void log(string message, LogEntrySeverityEnum severity)
        {
            TestLog.Write(message, severity);
        }

        protected void logDebug(string message)
        {
            TestLog.Write(message, LogEntrySeverityEnum.Debug);
        }

        #endregion

    }
}
