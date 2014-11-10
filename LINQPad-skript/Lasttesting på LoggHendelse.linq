<Query Kind="Program">
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Applikasjon.Owin.exe">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Applikasjon.Owin.exe</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Domene.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Applikasjon.Owin\bin\Debug\Bouvet.BouvetBattleRoyale.Domene.dll</Reference>
  <Reference Relative="..\Bouvet.BouvetBattleRoyale.Integrasjonstester\bin\Debug\Bouvet.BouvetBattleRoyale.Integrasjonstester.dll">C:\Projects\Bouvet\BBR\Bouvet.BouvetBattleRoyale.Integrasjonstester\bin\Debug\Bouvet.BouvetBattleRoyale.Integrasjonstester.dll</Reference>
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
  <Namespace>Bouvet.BouvetBattleRoyale.Domene</Namespace>
  <Namespace>Bouvet.BouvetBattleRoyale.Domene.Entiteter</Namespace>
  <Namespace>Bouvet.BouvetBattleRoyale.Domene.InputModels</Namespace>
  <Namespace>Bouvet.BouvetBattleRoyale.Integrasjonstester</Namespace>
  <Namespace>Microsoft.Owin.Hosting</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
</Query>

void Main()
{
	const int AntallKall = 100;
	
	const string LagId = "c0af3db";
	
	const string ApiBaseUrl = "http://bouvetbr.azurewebsites.net/";
		
	using (var client = new HttpClient())
    {
		string ApiMethodEndpointUrl = "/api/admin/lag/oppretthendelse";
		string ApiEndPointUrl = ApiBaseUrl + ApiMethodEndpointUrl;
	
		Enumerable.Range(1, AntallKall).AsParallel().ForAll(x=>
		{
			client.DefaultRequestHeaders.Authorization = TestManager.OpprettBasicHeader("bouvet", "mysecret");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			
			var modell = new LoggHendelseInputModell
			{
				LagId = LagId,
				Kommentar = "Testkommentar",
				HendelseType = HendelseType.Achievement
			};
			
			var modellSomJson = JsonConvert.SerializeObject(modell);
			
			var response = client.PostAsync(
				ApiEndPointUrl,
				new StringContent(modellSomJson, Encoding.UTF8, "application/json")).Result;

			response.IsSuccessStatusCode.Dump();
		});
	}
}