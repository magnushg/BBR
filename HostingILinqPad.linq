<Query Kind="Program">
  <Reference Relative="BouvetCodeCamp\bin\Debug\BouvetCodeCamp.exe"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\Microsoft.Owin.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\Microsoft.Owin.Host.HttpListener.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\Microsoft.Owin.Hosting.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\Newtonsoft.Json.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\Owin.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\System.Net.Http.Formatting.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\System.Web.Cors.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\System.Web.Http.Cors.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\System.Web.Http.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\System.Web.Http.Owin.dll"></Reference>
  <Reference Relative="BouvetCodeCamp\bin\Debug\System.Web.Http.Tracing.dll"></Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Namespace>Microsoft.Owin.Hosting</Namespace>
  <Namespace>BouvetCodeCamp</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

void Main()
{
	string baseAddress = "http://localhost:2014/";
	using (WebApp.Start<Startup>(baseAddress))
	{
		Console.WriteLine("Server running at {0}", baseAddress);
		Console.WriteLine("\r\nPress any key to stop server...");
		Console.ReadLine();
	}
}

// Define other methods and classes here
