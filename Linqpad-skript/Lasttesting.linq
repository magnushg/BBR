<Query Kind="Program">
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Applikasjon.Owin.exe">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Applikasjon.Owin.exe</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.Host.HttpListener.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.Host.HttpListener.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.Hosting.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Microsoft.Owin.Hosting.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Newtonsoft.Json.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Newtonsoft.Json.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Owin.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Owin.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Net.Http.Formatting.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Net.Http.Formatting.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Cors.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Cors.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Owin.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Owin.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Tracing.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\System.Web.Http.Tracing.dll</Reference>
  <Namespace>Bouvet.BouvetBattleRoyale.Applikasjon.Owin</Namespace>
  <Namespace>Microsoft.Owin.Hosting</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

void Main()
{
	const int AntallKall = 5;
	
	const string LagId = "c0af3db";
	
	const string ApiBaseUrl = "http://localhost:2014/";
		
	using (var client = new HttpClient())
    {
		string ApiMethodEndpointUrl = "/api/game/pif/hentmeldinger/" + LagId;
		string ApiEndPointUrl = ApiBaseUrl + ApiMethodEndpointUrl;
	
		Enumerable.Range(1, AntallKall).AsParallel().ForAll(x=>
		{
			var response = client.GetAsync(ApiEndPointUrl).Result;
			response.IsSuccessStatusCode.Dump();
		});
	}
}