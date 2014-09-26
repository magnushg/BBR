namespace BouvetCodeCamp.Lasttesting
{
    using System;
    using System.Net.Http;
    using System.Web.Http;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public interface IApiApplication
    {
        void Start();
    }

    public class InMemoryApiServer : IApiServer
    {
        private HttpServer _server;

        public Uri BaseAddress { get { return new Uri("http://localhost"); } }

        public ApiServerHost Kind
        {
            get { return ApiServerHost.InMemory; }
        }

        public HttpMessageHandler ServerHandler { get { return _server; } }

        public void Start()
        {
            try
            {
                var httpConfig = new HttpConfiguration();
                var apiConfig = new ApiServiceConfiguration(httpConfig);
                apiConfig.Configure();
                _server = new HttpServer(httpConfig);
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not create server: {0}", e);
                Assert.Fail("Could not create server: {0}", e);
            }
        }

        public void Stop()
        {
            try
            {
                _server.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not stop server: {0}", e);
            }
        }
    }

}