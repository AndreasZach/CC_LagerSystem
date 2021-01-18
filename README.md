# LagerSystem

Andreas Zachrisson

# Repository Pattern (DataRepository.cs)
Använde mig av ett simpelt Repository pattern av två andledningar:
	 1. Jag ville separera min business logik för att hantera data från min kontroller, så att min kontroller följer Single Responsibility principle.
	 2. Genom att lägga ansvaret för datahantering på ett repository, kan jag enklare implementera Open-Close principle till en rimlig nivå.
	
	(Lägg in kort beskrivning av Visitor / decorator)
	Jag Skulle kunna ha använt mig av Visitor/Decorator pattern för att hantera lagerstatus, samt logik relaterat till att 
		hantera eventuella saker som måste hanteras (ex kolla så att items inte hamnar negativt i count.
		Dock ansåg jag att ett Repository pattern som använder sig av foreach/Linq uttryck (Inbyggda visitor patterns egentligen), 
		var ett bättre val överlag av de anledningar jag givit ovan.
		
# Instruktioner
Se till at Cypress är installerat genom att köra "npm install Cypress --save-dev" kommando i Kommandotolken.
Starta appen genom Docker-Compose filen.
Kör E2E tester via "npm test" kommando i Kommandotolken.


