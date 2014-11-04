using Autofac;

using AutofacContrib.DynamicProxy;

using Bouvet.BouvetBattleRoyale.Applikasjon.Owin.Interception;
using log4net;
using Microsoft.Azure.Documents;

using Moq;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Bouvet.BouvetBattleRoyale.Integrasjonstester.Infrastructure
{
    using NUnit.Framework;

    [TestFixture]
    public class RetryInterceptionTests
    {
        private IContainer _container;

        private const int _maxAttempts = 3;

        [SetUp]
        public void FørHverTest()
        {
            var builder = new ContainerBuilder();

            var log = new Mock<ILog>();

            builder.RegisterInstance(new RetryInterceptor (log.Object){ MaxAttempts = _maxAttempts }).AsSelf().SingleInstance();
            builder.RegisterInstance(new SynchronousRetryInterceptor { MaxAttempts = _maxAttempts }).AsSelf().SingleInstance();
            builder.RegisterType<TestController>().EnableClassInterceptors().InterceptedBy(typeof(RetryInterceptor));

            _container = builder.Build();
        }

        private TestController HentTestController()
        {
            var controller = _container.Resolve<TestController>();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            return controller;
        }

        [Test]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_annen_DocumentClientException_Skal_ikke_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagDocumentClientException("Not PreconditionFailed");

            try
            {
                controller.SendInputSynkron("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(1, controller.AntallKall, "Skulle ikke prøvd på nytt");
        }

        [Test]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_Skal_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagDocumentClientException();

            try
            {
                controller.SendInputSynkron("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall-1, "Skulle prøvd på nytt");
        }

        [Test]
        public async Task AsynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_Skal_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagDocumentClientException();

            try
            {
                await controller.SendInputAsync("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall - 1, "Skulle prøvd på nytt");
        }

        [Test]
        public async Task AsynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_og_deretter_fungerer_Skal_gi_retry_og_returverdi()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagDocumentClientException();
            controller.ThrowTimes = 1;

            HttpResponseMessage response = null;

            try
            {
                response = await controller.SendInputAsync("input"); 
            }
            catch (Exception ex) 
            {
                Assert.Fail("Skulle ikke kastet Exception");
            }

            string value;
            response.TryGetContentValue<string>(out value);

            Assert.IsNotNull(response, "Response ble ikke satt");
            Assert.AreEqual("input", value, "Feil innhold");
            Assert.AreEqual(2, controller.AntallKall, "Skulle prøvd på nytt en gang");            
        }

        [Test]
        public async Task AsynkronMetode_med_HttpPost_Når_ingen_exceptions_Skal_gi_returverdi()
        {
            var controller = HentTestController();

            HttpResponseMessage response = null;

            try
            {
                response = await controller.SendInputAsync("input");                
            }
            catch (Exception ex)
            {
                Assert.Fail("Skulle ikke kastet Exception");
            }

            string value;
            response.TryGetContentValue<string>(out value);

            Assert.IsNotNull(response, "Response ble ikke satt");
            Assert.AreEqual("input", value, "Feil innhold");
            Assert.AreEqual(1, controller.AntallKall, "Skulle bare kjørt en gang");
        }

        [Test]
        public async Task AsynkronVoidMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_Skal_gi_retry()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagDocumentClientException();

            try
            {
                await controller.SendInputVoidAsync("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall - 1, "Skulle prøvd på nytt");
        }

        [Test]
        public async Task AsynkronVoidMetode_med_HttpPost_Når_den_kaster_en_ConcurrencyException_før_den_funker_Skal_gi_en_retry_og_returverdi()
        {
            var controller = HentTestController();
            controller.ThrowThis = LagDocumentClientException();
            controller.ThrowTimes = 1;
            
            try
            {
                await controller.SendInputVoidAsync("input");
               
            }
            catch (Exception ex) 
            {
                Assert.Fail("Skulle ikke kastet Exception");
            }

            Assert.AreEqual(2, controller.AntallKall, "Skulle prøvd på nytt");
        }

        [Test]
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

        [Test]
        public void SynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_med_TimeSpan_Skal_gi_retry_og_vente()
        {
            var controller = HentTestController();

            var retryAfterMs = 50;

            controller.ThrowThis = LagDocumentClientException("PreconditionFailed", retryAfterMs);

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

        [Test]
        public async Task AsynkronMetode_med_HttpPost_Når_den_kaster_ConcurrencyException_med_TimeSpan_Skal_gi_retry_og_vente()
        {
            var controller = HentTestController();

            var retryAfterMs = 50;

            controller.ThrowThis = LagDocumentClientException("PreconditionFailed", retryAfterMs);

            var stopwatch = Stopwatch.StartNew();
            try
            {
                await controller.SendInputAsync("input");
                Assert.Fail("Skulle kastet Exception");
            }
            catch (Exception ex) { }

            Assert.AreEqual(_maxAttempts, controller.AntallKall - 1, "Skulle prøvd på nytt");
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > _maxAttempts * retryAfterMs, "Skulle ventet mellom hver retry");
        }

        [Test]
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

        [Test]
        public void SynkronMetode_med_HttpPost_Når_den_ikke_kaster_Exception_Skal_ikke_gi_retry()
        {
            var controller = HentTestController();

            HttpResponseMessage response = null;

            try
            {
                response = controller.SendInputSynkron("input");
            }
            catch (Exception ex) 
            {
                Assert.Fail("Skulle ikke kastet Exception");
            }

            string value;
            response.TryGetContentValue<string>(out value);

            Assert.IsNotNull(response, "Response ble ikke satt");
            Assert.AreEqual("input", value, "Feil innhold");
            Assert.AreEqual(1, controller.AntallKall, "Skulle ikke prøvd på nytt");
        }

        [Test]
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

        private Exception LagDocumentClientException(string code = "PreconditionFailed", int retryAfterMs = 0) 
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
            public int ThrowTimes { get; set; }
            private int _thrownCount;

            private void KastExceptionHvisSatt() 
            {                
                AntallKall++;

                if (ThrowThis != null && (ThrowTimes == 0 || _thrownCount < ThrowTimes))
                {
                    _thrownCount++;
                    throw ThrowThis; 
                }
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
