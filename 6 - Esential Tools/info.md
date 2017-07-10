|Problem|Solution|
|-------|--------|
|Decouple classes|Introduce interfaces and declare dependencies on them in the class constructor|
|Automatically resolve dependencies expressed using interfaces|Use Ninject or another dependency injection container|
|Integrate Ninject into an MVC application|Create an implementation of the IDependencyResolver interface that calls teh Ninject kernel and register it as a resolver by calling the System.Web.Mvc.DependencyResolver.SetResolver method|
|Inject property and constructor values into newly created objects|Use the WithPropertyValue and WithConstructorArgument methods|
|Dynamcally select an implementation class for an interface|Use an Ninject conditional binding|
|Control the lifecycle of the objects that Ninject creates|Set an object scope|
|Create a unit test|Add a unit test project to the solution and annotate a class file with TestClass and TestMethod attributes|
|Check for expected outcomes in a unit test|Use the Assert class|
|Focus a unit test on a single feature of component|Isolate he test target using mock objects|

