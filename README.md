# CommercialOptimiser

Uses pre-loaded Break and Commercial data to allow a user to select commercials to be allocated in the available breaks in the optimal way.

<b>Screenshots</b>:<br/>
![Alt text](/Screenshots/Commercials.png?raw=true "Commercial Selection")<br/>
![Alt text](/Screenshots/AllocationReport.png?raw=true "Allocation Report")<br/>

<b>Projects</b>:<br/>
CommercialOptimiser.App - ASP.Net Web App using Blazor<br/>
CommercialOptimiser.Api - Api Web Service<br/>
CommercialOptimiser.Core - Shared models<br/>
CommercialOptimiser.Data - Database integration with table definitions and Entity Framework retrieval factories. Note - it is currently configured for MS Sql Server.<br/>

<b>Configuration</b>:<br/>
CommercialOptimiser.App/appsettings.json - ApiBaseUrl should be set to the hosted url of CommercialOptimiser.Api<br/>
CommercialOptimiser.Data/appsettings.json - CommercialOptimiser connection string should be setup based on the location of your database<br/>

<b>Note</b>: The solution supports different combinations of breaks, break capacities, invalid commercial types etc. To try these either edit CommercialOptimiser.Data/DatabaseInitializer and re-create the database or change the table contents directly via SQL.


