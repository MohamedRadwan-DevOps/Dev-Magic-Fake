<div class="wikidoc">
<p><strong>Abstraction&nbsp;</strong></p>
<p>DevMagicFake will enable us to save and retrieve any object of our domain model without writing any code, it stores and retrieves objects using memory. DevMagicFake also will enable us to generate objects with data, so no need to create any faked or mock
 object to simulate our application anymore, so by using DevMagicFake we can implement the concept of PI (Persistence Ignorance) of the DDD, by using DevMagicFake we don’t need to create DB or ORM to finish our application, by using DevMagicFake we can
 complete our application without writing the part of the persistence data, this will enable us to work toward verifying the business requirements and will give birth to real TDD, save our time and effort from writing faking and mocking code that we really
 don’t need it.</p>
<p>It is better to download the library using NuGet for the desired project, this will add the default configuration to the Web.config or the App.config.</p>
<p><a title="DevMagicFake on NuGet" href="http://nuget.org/List/Packages/DevMagicFake" target="_blank"><img src="https://public.bay.livefilestore.com/y1pTTQh4X67a6BhoB7ZooPKOdTA1sYwH7gJ9bNaBp8yKB7xhAe-esVBV2T81WjFrAqJvJmVXRBe_dekqwv-FcjOoQ/nugetlogo.png?psid=1" alt="NuGet" style="vertical-align:middle" width="188" height="59"></a>&nbsp;&nbsp;<span style="font-family:Consolas; font-size:21px; line-height:24px">PM&gt;
 Install-Package DevMagicFake</span></p>
<p><span>&nbsp;</span></p>
<p>Let’s see some code snippet about some little features of DevMagicFake</p>
<p><span style="color:#ff0000"><strong>Note:</strong>&nbsp;All domain model classes must be resident inside a class library project (Assembly) it's name is&nbsp;<strong>Domain</strong>&nbsp;and the main project add&nbsp;reference&nbsp;to it, this if you want
 to use the default configuration otherwise you need to change the assembly name in the&nbsp;configuration&nbsp;file</span></p>
<p>&nbsp;</p>
<ul>
<li>Generate 3 objects and generate its data for any class exists in our domain model assembly
</li></ul>
<div style="color:black; background-color:white">
<pre style="padding-left:60px"><span><span style="color:blue">var</span> </span>repository = <span style="color:blue">new</span> FakeRepository();<br>repository<span>.GenerateDataForAllAssemblyTypes(3);</span></pre>
</div>
<div style="color:black; background-color:white"><br>
<ul style="color:black">
<li>Get all objects for any type from our domain model, for example get all customers
</li></ul>
<div style="color:black; background-color:white">
<pre style="padding-left:60px"><span><span style="color:blue">var</span> </span>repository = <span style="color:blue">new</span> FakeRepository&lt;Customer&gt;();<br><span><span style="color:blue">var</span>  customerList = repository. GetAll();
</span></pre>
</div>
<br>
<br>
<ul style="color:black">
<li>Save any object of any type, for example create and save customer </li></ul>
<p>&nbsp;</p>
<div style="color:black; background-color:white">
<pre style="padding-left:30px">    <span style="color:blue">var</span> customer = <span style="color:blue">new</span> Customer
                {
                    <span style="color:green">//Id = 1 -- No need, DevMagicFake will assign incremental Id per type, if it's new object</span>
                    Name = <span style="color:#a31515">"Mohamed Radwan"</span>,
                    Email = <span style="color:#a31515">"MRadwan@devmagicfake.com"</span>
                };

    var repository = new FakeRepository<Customer>();
    repository.Save(customer);
</pre>
</div>
<p>&nbsp;</p>
</div>
<div style="color:black; background-color:white">
<ul style="color:black">
<li>Retrieve any aggregate root object by id like customer </li></ul>
<div style="color:black; background-color:white">
<pre style="padding-left:60px"><span><span style="color:blue">var</span> </span>repository = <span style="color:blue">new</span> FakeRepository&lt;Customer&gt;();<br><span><span style="color:blue">var</span>  customer = </span>repository.GetById(1);</pre>
</div>
</div>
<div style="color:black; background-color:white"><br>
<ul style="color:black">
<li>Create faked objects from any type and generate their data, for example create 4 customer and generate its data
</li></ul>
<div style="color:black; background-color:white">
<pre style="padding-left:60px"><span><span style="color:blue">var</span> </span>repository = <span style="color:blue">new</span> FakeRepository&lt;Customer&gt;();<br><span><span style="color:blue">var</span>  customerList = </span>repository.Create(4)<br><span>
</span></pre>
</div>
</div>
<div style="color:black; background-color:white"><br>
<ul style="color:black">
<li>There are&nbsp;many&nbsp;other features that exist in DevMagicFake, for more information see the feature list and the tutorial
</li></ul>
<p style="color:black">&nbsp;</p>
</div>
<p><strong>Project Description</strong></p>
<p>DevMagicFake is a faking framework, it developed in C#, it enable developers to isolate the UI from the underline layers specially for MVC projects or any other project that use DDD or repository pattern, it gives developers the ability to focus on how to
 complete, verify and test the application behaviors and response without coding and without focus on developing the underline layers, until the application features completed, tested and approved</p>
<p>DevMagicFake developed mainly to work very well with ASP MVC web applications, but it can work as well with other applications.</p>
<p>DevMagicFake, give us the ability to implement TDD Test Driven Development through provide the right behaviors of the system which are the main input for test driven approach.</p>
<p>DevMagicFake can simulate the data model, so you don't need to create any CRUD by code, it's like data access application, it is working like the mock object in mocking framework with unit testing framework,</p>
<p>it provides Fakeable operations for all needed activities for developers in which allow them to run the system features as if they complete its programming, so that the feature can be tested by QC for functional requirements and can be verified by the client
 against the acceptance criteria and if it meets its requirements or not.</p>
<p>&nbsp;</p>
<p><strong>Links</strong></p>
<ul>
<li><a title="Video Tutorial" href="http://mohamedradwan.com/2011/09/03/dev-magic-fake-video-tutorial/" target="_blank">Dev Magic Fake Video Tutorial</a>
</li></ul>
<ul>
<li><a title="MVC Project" href="/misc/TryDevMagicFake.rar" target="_blank">MVC 3.0 Project that use Dev Magic Fake (TryDevMagicFake)</a>
</li></ul>
<ul>
<li><a title="Dev Magic Fake story" href="http://mohamedradwan.com/2011/09/03/the-reasons-of-creating-and-naming-dev-magic-fake/" target="_blank">Introduction to Dev Magic Fake</a>
</li></ul>
<ul>
<li><a title="Uderstand Dev Magic Fake " href="/misc/Understanding Dev Magic Fake.pdf" target="_blank">Understanding Dev Magic Fake PDF</a>
</li></ul>
<ul>
<li><a title="Dev Magic Fake Tutorial" href="/misc/How to use Dev Magic Fake Tutorial 1.1.pdf" target="_blank">Tutorial of how to use Dev Magic Fake PDF&nbsp;</a>
</li></ul>
<ul>
<li><a title="Dev Magic Fake Help Fle" href="/misc/DevMagicFake.chm" target="_blank">Dev Magic Fake Help file CHM</a>
</li></ul>
<ul>
<li><a title="Presention of Dev Magic Fake features" href="/misc/Introduction to Dev Magic Fake Framework.pptx" target="_blank">Dev Magic Fake quick introduction presentation PPTX&nbsp;</a>
</li></ul>
<ul>
<li><a title="Presention of Dev Magic Fake features" href="/misc/DevMagicFakeSnippet.msi" target="_blank">Faking Snippets</a>
</li></ul>
<p>&nbsp;</p>
<p><strong>The main goals of Dev Magic Fake are to give us the ability to:</strong></p>
<ul>
<li>Focus on what rather than focus on how </li><li>Provide real implementation of abstraction in software development </li><li>Implementing “Develop By Feature” approach by Agile methodology </li><li>Complete the feature without coding the underline layers </li><li>Give the ability to creating a passed successful unit testing to test the behavior and response of the application without completing the underline layers
</li><li>Give the ability to creating a passed successful UI test to test the behavior and response of the UI without complete the underline layers
</li><li>Create faking code with no effort </li><li>Create faking code in no time </li><li>Permanent data storage </li><li>Minimum effort for replacing the faking code with the real one </li></ul>
<p>Dev Magic Fake not just code framework, Dev Magic Fake is a software development approach for the Agile methodology, Dev Magic Fake working better with TDD and MVC pattern</p>
<p>&nbsp;</p>
<p><strong>Dev Magic Fake Feature List</strong></p>
<ul>
<li>Easy implementation and usage without any prerequisite </li><li>Save any simple instance </li><li>Retrieve any instance of any type by Id </li><li>Save any complex instance (container of other instances) and retrieve its nested instance
</li><li>Save any complex instance and save its nested instances </li><li>Save any complex instance (container of other instances) that has collection and retrieve it’s items
</li><li>Save any complex instance that has a collection and save the collection items
</li><li>Get all saved instances of any class </li><li>Generate instances for all classes in an assembly and generate it’s data
</li><li>Generate instances for all classes that saved </li><li>Use the default data generation mechanism </li><li>Control the data generation mechanism using Range </li><li>Control the data generation mechanism using data types </li><li>Control the data generation that just included in specific assembly </li><li>Control the data generation that just included in specific namespace </li><li>Control the data generation that just marked with Fakeable attribute </li><li>Control the data generation to eliminate any type marked with NotFakeable attribute
</li><li>Control the data generation for the depth of the object graph </li><li>Create any instance of any type and generate its data </li><li>Create list of instance of any type and generate its data </li><li>Provide permanent saved data for all saved instances </li><li>Easily query Dev Magic Fake using LINQ </li><li>Fluent interface for configuration and data generation </li></ul>
<p><strong>Dev Magic Fake Road Map and Upcoming Features</strong></p>
<ul>
<li>Support data generation based on data annotation </li><li>Support data generation based on regular expression </li><li>Support data generation based on database tables </li><li>Support many to many types in an effective and better way </li><li>Support custom collection </li><li>Support random enumeration </li><li>Using UI for configuration </li><li>Eliminate or reduce the maintainability of the faking code by using DI (Dependency Injection) and IoC (Inversion of Control).
</li></ul>
<p>&nbsp;</p>
<p><strong>Founder and creator</strong></p>
<p>Dev Magic Fake authored and created by M.Radwan</p>
<p><a href="http://mohamedradwan.com/">http://mohamedradwan.com</a></p>
<p>Please contact me if you want to contribute to this project on</p>
<p><a title="email" href="mailto:mradwan.automationplanet@gmail.com" target="_blank">mradwan.automationplanet@gmail.com</a></p>
<p>Thanks</p>
</div>
</div>
