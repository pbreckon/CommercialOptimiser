# CommercialOptimiser

Uses pre-loaded Break and Commercial data to allow a user to select commercials to be allocated in the available breaks in the optimal way.

<b>Screenshots</b>:<br/>
![Alt text](/Screenshots/Commercials.png?raw=true "Commercial Selection")<br/>
![Alt text](/Screenshots/AllocationReport.png?raw=true "Allocation Report")<br/>

<b>Projects</b>:<br/>
<ul>
  <li>CommercialOptimiser.App - ASP.Net Web App using Blazor</li>
  <li>CommercialOptimiser.Api - Api Web Service</li>
  <li>CommercialOptimiser.Core - Shared models</li>
  <li>CommercialOptimiser.Data - Database integration with table definitions and Entity Framework retrieval factories. Note - it is currently configured for MS Sql Server.</li>
</ul>
  
<b>Configuration</b>:<br/>
<ul>
  <li>CommercialOptimiser.App/appsettings.json - ApiBaseUrl should be set to the hosted url of CommercialOptimiser.Api</li>
  <li>CommercialOptimiser.Data/appsettings.json - CommercialOptimiser connection string should be setup based on the location of your database</li>
</ul>

<b>Note</b>: The solution supports different combinations of breaks, break capacities, invalid commercial types etc. To try these either edit CommercialOptimiser.Data/DatabaseInitializer and re-create the database or change the table contents directly via SQL.


