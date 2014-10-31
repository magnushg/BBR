<Query Kind="Program">
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Applikasjon.Owin.exe">Bouvet.BouvetBattleRoyale.Applikasjon.Owin.exe</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.dll">Microsoft.Owin.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.Host.HttpListener.dll">Microsoft.Owin.Host.HttpListener.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.Hosting.dll">Microsoft.Owin.Hosting.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Newtonsoft.Json.dll">Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Owin.dll">Owin.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Net.Http.Formatting.dll">System.Net.Http.Formatting.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Cors.dll">System.Web.Cors.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.dll">System.Web.Http.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Owin.dll">System.Web.Http.Owin.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Owin.dll">System.Web.Http.Owin.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Tracing.dll">System.Web.Http.Tracing.dll</Reference>
  <Namespace>Bouvet.BouvetBattleRoyale.Applikasjon.Owin</Namespace>
  <Namespace>Microsoft.Owin.Hosting</Namespace>
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