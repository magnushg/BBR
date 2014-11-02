using Autofac;
using Autofac.Core;
using AutofacContrib.DynamicProxy;
using Bouvet.BouvetBattleRoyale.Applikasjon.Owin;
using Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception;
using Microsoft.Azure.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastructure
{
    [TestClass]
    public class RetryInterceptionTests
    {
        private IContainer _container;
        private int _maxAttempts = 3;

        [TestInitialize]
        public void FørHverTest()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(new RetryInterceptor { MaxAttempts = _maxAttempts }).AsSelf().SingleInstance();
            builder.RegisterInstance(new SynchronousRetryInterceptor { MaxAttempts = _maxAttempts }).AsSelf().SingleInstance();
            builder.RegisterType<TestController>().EnableClassInterceptors().InterceptedBy(typeof(RetryInterceptor));
            _container = builder.Build();
        }

        private TestController HentTestController()
        {
            return _container.Resolve<TestController>();
        }

        [TestMethod]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_annen_DocumentClientException_Skal_ikke_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagConcurrencyException("Not PreconditionFailed");

            try
            {
                controller.SendInputSynkron("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(1, controller.AntallKall, "Skulle ikke prøvd på nytt");
        }

        [TestMethod]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_Skal_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagConcurrencyException();

            try
            {
                controller.SendInputSynkron("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall-1, "Skulle prøvd på nytt");
        }

        [TestMethod]
        public async Task AsynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_Skal_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagConcurrencyException();

            try
            {
                await controller.SendInputAsync("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall - 1, "Skulle prøvd på nytt");
        }

        [TestMethod]
        public async Task AsynkronVoidMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_Skal_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagConcurrencyException();

            try
            {
                await controller.SendInputVoidAsync("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall - 1, "Skulle prøvd på nytt");
        }

        [TestMethod]
        public async Task AsynkronVoidMetode_med_HttpPost_Når_den_kaster_Annen_Exception_Skal_ikke_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = new ApplicationException("Ikke retry application exception");

            try
            {
                await controller.SendInputVoidAsync("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(1, controller.AntallKall, "Skulle ikke prøvd på nytt");
        }

        [TestMethod]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_med_TimeSpan_Skal_gi_retry_og_vente()
        {
            var controller = HentTestController();

            var retryAfterMs = 50;

            controller.ThrowThis = LagConcurrencyException("PreconditionFailed", retryAfterMs);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                controller.SendInputSynkron("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }            

            Assert.AreEqual(_maxAttempts, controller.AntallKall - 1, "Skulle prøvd på nytt");
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > _maxAttempts * retryAfterMs, "Skulle ventet mellom hver retry");
        }

        [TestMethod]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_AnnenException_Skal_ikke_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = new ApplicationException("Ikke retry application exception");

            try
            {
                controller.SendInputSynkron("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) {}

            Assert.AreEqual(1, controller.AntallKall, "Skulle ikke prøvd på nytt");
        }

        [TestMethod]
        public void SynkronMetode_uten_HttpPost_Når_den_kaster_AnnenException_Skal_ikke_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = new ApplicationException("Ikke retry application exception");

            try
            {
                controller.HentInput("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(1, controller.AntallKall, "Skulle ikke prøvd på nytt");
        }

        private Exception LagConcurrencyException(string code = "PreconditionFailed", int retryAfterMs = 0) 
        {
            var error = new Error { Code = code, Message = "Etag didn't match" };
            HttpResponseHeaders headers = null;

            var concurrencyException = (DocumentClientException)Activator.CreateInstance(typeof(DocumentClientException), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { error, headers }, Thread.CurrentThread.CurrentCulture);

            concurrencyException.ResponseHeaders.Add("x-ms-retry-after-ms", retryAfterMs.ToString());
            concurrencyException.StatusCode = HttpStatusCode.PreconditionFailed;
           
            return new Exception("Sjekk innerException", concurrencyException);
        }

        public class TestController : ApiController
        {
            public Exception ThrowThis { get; set; }
            public int AntallKall { get; set; }

            private object _lock = new object();

            private void KastExceptionHvisSatt() 
            {
                lock (_lock)
                {
                    AntallKall++;
                }
                if (ThrowThis != null)
                    throw ThrowThis;
            }

            [HttpGet]
            public virtual HttpResponseMessage HentInput(string input)
            {
                KastExceptionHvisSatt();
                return Request.CreateResponse(HttpStatusCode.OK, input, Configuration.Formatters.JsonFormatter);
            }

            [HttpPost]
            public virtual HttpResponseMessage SendInputSynkron(string input)
            {
                KastExceptionHvisSatt();
                return Request.CreateResponse(HttpStatusCode.OK, input, Configuration.Formatters.JsonFormatter);
            }

            [HttpPost]
            public virtual async Task SendInputVoidAsync(string input)
            {
                KastExceptionHvisSatt();                
            }

            [HttpPost]
            public virtual async Task<HttpResponseMessage> SendInputAsync(string input)
            {
                KastExceptionHvisSatt();
                return Request.CreateResponse(HttpStatusCode.OK, input, Configuration.Formatters.JsonFormatter);
            }
        }
    }
}
