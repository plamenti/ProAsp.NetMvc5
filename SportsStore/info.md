|Project Name| Visual Studio Project Template | Purpose|
|------------|--------------------------------|--------|
|SportsStore.Domain|Class Library|Holds the domain entities ad logic; set up for persistence via a repository created whit teh Entity Framework|
|SportsStore.WebUI|ASP.NET MVC Web Application (choose Empty when prompted to choose a project template and check teh MVC option)|Holds the controllers and views; acts as the UI for the SportsStore application|
|SportsStore.UnitTests|Unit Test Project|Holds the unit tests for the other two projects|

|Project Name| Solution Dependencies | Assemblies References|
|------------|--------------------------------|--------|
|SportsStore.Domain|None|System.ComponentModel.DataAnnotations|
|SportsStore.WebUI|SportsStore.Domain|None|
|SportsStore.UnitTests|SportsStore.Domain SportsStore.WebUI|System.Web, Microsoft.CSharp|

Shopping Cart


