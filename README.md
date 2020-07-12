# CommercialOptimiser

Uses pre-loaded Break and Commercial data to allow a user to select commercials to be allocated in the available breaks in the optimal way.

<b>Screenshots</b>:<br/>
![Alt text](/Screenshots/Commercials.png?raw=true "Commercial Selection")<br/>
![Alt text](/Screenshots/AllocationReport.png?raw=true "Allocation Report")<br/>

<b>Projects</b>:<br/>
<ul style="list-style-position:inside;">
  <li>CommercialOptimiser.App - ASP.Net Web App using Blazor</li>
  <li>CommercialOptimiser.Api - Api Web Service</li>
  <li>CommercialOptimiser.Core - Shared models</li>
  <li>CommercialOptimiser.Data - Database integration with table definitions and Entity Framework retrieval factories. Note - it is currently configured for MS Sql Server.</li>
  <li>CommercialOptimiser.Api.Test - Unit tests for the optimising logic
</ul>
  
<b>Configuration</b>:<br/>
<ul style="list-style-position:inside;">
  <li>CommercialOptimiser.App/appsettings.json - ApiBaseUrl should be set to the hosted url of CommercialOptimiser.Api</li>
  <li>CommercialOptimiser.Data/appsettings.json - CommercialOptimiser connection string should be setup based on the location of your database</li>
  <li>You may have to create the database yourself on SQL Server (name it "CommercialOptimiser". The App will create the tables/fill the data for you though</li>
</ul>

<b>Notes</b>: 
<ul style="list-style-position:inside;">
  <li>The solution supports different combinations of breaks, break capacities, invalid commercial types etc. To try these either edit CommercialOptimiser.Data/DatabaseInitializer and re-create the database or change the table contents directly via SQL.</li>
  <li>Since the solution is data-driven it should be easy to enhance in the future, e.g. to allow the user to specify Breaks with varying capacities, Commercials and Demographics</li>
  <li>All projects were created using .NET Core, so there should be no problems installing it on Windows, Unix, AWS etc.</li>
  <li>For mobile a native app would be preferrable, however due to time constraints this was not possible</li>
</ul>

