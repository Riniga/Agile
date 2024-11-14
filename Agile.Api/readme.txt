Förutsättningar
---------------
func installeras med: npm i -g azure-functions-core-tools@3 --unsafe-perm true
Cosmos DB Emulator
https://docs.microsoft.com/sv-se/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21

Utvecklingsmiljöer (för test och utveckling)
--------------------------------------------
starta med: func start --csharp
Debug genom att "Attach to process" -> välj func bland processer

Konfiguration som måste finnas i local.settings.json
Jag använder Cosmos DB emulator med följande konfiguration...
  "Values": {
    "EndpointUrl": "https://localhost:8081",
    "PrimaryKey": "[]",
    "DatabaseId": "NissesGagnerDatabase",
    "ContainerId": "DevopsHealthRadar"
  }

Produktionsmiljö 
----------------
Jag använder en publish profile, högerklicka och publish för att produktionssätta.
Aktuell miljö: SkanskaResourceGroup -> devopshealthradar (Function App)


