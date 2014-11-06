Bouvet Battle Royale
====================

Bouvet Battle Royale er en spillplattform som tilbyr et API til et slags orienteringsløp hvor man samler poster og koder underveis.

# Arkitektur #

## Domenemodell ##

- TODO: link til gliffy

## Arkiveringsstrategi ##

Underveis i spillet opprettes det mange meldinger, posisjoner og logghendelser for hvert av lagene. Disse tilhører et Lag men samtidig ønskes det å holde Lag-dokumentet så lean som mulig. Derfor har ikke Lag lister med disse objekttypene men istedet opprettes det enkeltdokumenter som har en *soft-reference* på **LagId**. 

### Worker ###
Disse objektypene blir sendt til et "arkiv" så fort de er opprettet. Arkivet er en kø hvor det står en Worker og lytter på og plukker ned meldinger. Konseptet er tatt fra *Producer/Consumer*-patternet hvor man har en eller flere ivrige **Producers** av meldinger som legges i en kø, og en eller flere sultne **Consumers** som plukker meldinger ned fra køen. 

Meldingene i køen er egentlig bare objektene som er serialisert i `QueueMessageProducer` til json på formatet **type:json-objekt**. Det finnes en `QueueMessageConsumer` som deserialiserer og sender objektet videre til respektivt `Repository<T>` for lagring.

- TODO: link til gliffy

# Teknisk oppsett #

## Simulere spillflyt ##

todo: tekst om spillsimulator

## Opprette spilltilstand ##

todo: tekst om spilloppretter

## Hosting i LINQPad ##

Åpne skriptet HostingILINQPad.linq og kjør skriptet. For å fjerne log4net-advarsel om at LINQPad ikke finner log4net-konfig så gå til C:\Program Files (x86)\LINQPad4 og opprett en LINQPad.config-fil med log4net-konfigen fra Owin-prosjektets App.config.

## Integrasjonstester ##

### API-tester ###

Integrasjonstestene starter opp en OWIN-applikasjon mot [http://localhost:52501](http://localhost:52501) i `BaseApiTest`.
Testene bruker API'et til å sette opp data/tilstand før selve testen kjøres. Data/tilstand blir så fjernet etter testkjøring. Det brukes konsekvent en gitt LagId (Testlag1) for testing for å unngå å slette ekte data.

Testene kjører mot databasen som ligger i app.config i integrasjonstestprosjektet

### DataAksess-tester ###

Integrasjonstester mot Repositories oppretter hver sin Repository-implementasjon og kjører mot databasen definert i app.config. Før og etter hver test blir data/tilstand opprettet og slettet, se `BaseRepositoryIntegrasjonstest.cs`