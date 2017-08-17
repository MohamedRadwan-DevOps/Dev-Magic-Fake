// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepositoryTest.cs" company="http://mohamedradwan.wordpress.com">
//   © 2011 M.Radwan. All rights reserved
// </copyright>
// <summary>
//   This is a test class for RepositoryTest and is intended
//   to contain all RepositoryTest Unit Tests
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using M.Radwan.DevMagicFake;
using M.Radwan.DevMagicFake.Configuration;
using M.Radwan.DevMagicFake.DataGeneration;
using M.Radwan.DevMagicFake.Extensions;
using M.Radwan.DevMagicFake.FakeRepositories;
using M.Radwan.EntitiesTest;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace M.Radwan.DevMagicFakeTest
{
    /// <summary>
    /// This is a test class for RepositoryTest and is intended
    ///   to contain all RepositoryTest Unit Tests
    /// </summary>
    [TestClass]
    public class RepositoryTest
    {
        #region Constants and Fields

        /// <summary>
        ///   The framework settings.
        /// </summary>
        private static FrameworkSettings frameworkSettings;

        /// <summary>
        ///   The test context instance.
        /// </summary>
        private TestContext testContextInstance;

        #endregion

        #region Properties

        ///<summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;
            }
        }

        #endregion

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class

        #region Test Configuration
        /// <summary>
        /// The my class initialize.
        /// </summary>
        /// <param name="testContext">
        /// The test context.
        /// </param>
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            frameworkSettings = FrameworkSettings.FrameworkSettingsInstance;
            frameworkSettings.EntitiesAssembly = "EntitiesTest.dll";

        }

        /// <summary>
        /// Use TestCleanup to run code after each test has run
        /// </summary>
        [TestCleanup]
        public void MyTestCleanup()
        {
            MemoryStorage.MemoryDb = new Dictionary<string, List<dynamic>>();
            frameworkSettings.SetDefaultSettings();
            frameworkSettings.DataGenerationPrimaryRules.Clear();

        }

        /// <summary>
        /// The my test initialize.
        /// </summary>
        [TestInitialize]
        public void MyTestInitialize()
        {
            MemoryStorage.MemoryDb = new Dictionary<string, List<dynamic>>();
            frameworkSettings.SetDefaultSettings();
            frameworkSettings.EntitiesAssembly = "EntitiesTest.dll";
            frameworkSettings.RandomDynamicSeed = new Random((int)DateTime.Now.Ticks); // thanks to McAden
            frameworkSettings.RandomFixedSeed = new Random(3);
            frameworkSettings.CurrentRandom = frameworkSettings.RandomFixedSeed;
            frameworkSettings.MaximumObjectGraphLevel = 10000;
            frameworkSettings.UseFakeableAttribute = false;
            frameworkSettings.UseNotFakeableAttribute = false;
            frameworkSettings.DataGenerationPrimaryRules.Clear();

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Factory Creation Test method
        /// </summary>
        [TestMethod]
        public void FactoryCreation()
        {
            // Arrange
            var repFactory0 = new RepositoryFactory();
            var repFactory1 = new RepositoryFactory();
            var repFactory2 = new RepositoryFactory();
            var repFactory3 = new RepositoryFactory();

            // Act
            var repository0 = (FakeRepository)repFactory1.CreateRepository();
            var repository1 = (FakeRepository<Customer>)repFactory1.CreateRepository(typeof(Customer));
            var repository2 = (FakeRepository<ProductTypeForm, VendorForm>)repFactory2.CreateRepository(typeof(ProductTypeForm), typeof(VendorForm));
            var repository3 = (FakeRepository<ServiceForm, VendorForm, ProductTypeForm>)repFactory3.CreateRepository(typeof(ServiceForm), typeof(VendorForm), typeof(ProductTypeForm));

            Assert.IsNotNull(repository0);
            Assert.IsNotNull(repository1);
            Assert.IsNotNull(repository2);
            Assert.IsNotNull(repository3);
        }

        /// <summary>
        /// The call two times property name and generate from list of integer and strings and null reference has value.
        /// </summary>
        [TestMethod]
        public void CallTwoTimesPropertyNameAndGeneratFromListOfIntegerAndStringsAndNullRefernceHasValue()
        {
            // Arrange
            var nums = new List<int> { 1, 5, 20, 254 };
            var emails = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<Customer>();
            repository.RuleUsesPropertyOnly(p=>"Id", o => o.GeneratFromList(nums)).RuleUsesPropertyOnly(p=>"Email", o => o.GeneratFromList(emails).NullPercentage(0.4));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value1;
            var hasValue1 = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Id", out value1);

            string value2;
            var hasValue2 = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Email", out value2);

            Assert.IsTrue(hasValue1);
            Assert.IsTrue(value1 == "1|5|20|254|Null:0|GenerationType:List");
            Assert.IsTrue(hasValue2);
            Assert.IsTrue(value2 == "Radwan|Seif|Lara|Rania|Null:0.4|GenerationType:List");

        }

        /// <summary>
        /// The class property with generation type list no null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeListNoNull()
        {
            // Arrange
            var names = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Name, o => o.GeneratFromList(names));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Name", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0|GenerationType:List");
        }

        /// <summary>
        /// The Class property with generation type list with null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeListWithNull()
        {
            // Arrange
            var names = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Name, o => o.GeneratFromList(names).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Name", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0.5|GenerationType:List");
        }

        /// <summary>
        /// The Class property with generation type random no null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeRandomNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromRandom<int>());

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0|GenerationType:Random");
        }
        /// <summary>
        /// The class property With Generation Type Random With Parameter No Null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeRandomWithParameterNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromRandom(d => d.Int));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0|GenerationType:Random");
        }

        /// <summary>
        /// The Class property with generation type random with null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeRandomWithNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromRandom<int>().NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0.5|GenerationType:Random");
        }

        /// <summary>
        /// The Class property with generation type range no null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeRangeNoNull()
        {
            // Arrange
            int from = 1;
            int to = 1000;

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromRange(from, to));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|1000|Null:0|GenerationType:Range");
        }

        /// <summary>
        /// The Class property with generation type range with null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeRangeWithNull()
        {
            // Arrange
            int from = 1;
            int to = 1000;

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromRange(from, to).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|1000|Null:0.5|GenerationType:Range");
        }

        /// <summary>
        /// The Class property with generation type value no null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeValueNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromValue(12345, 4));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "12345|Null:0|GenerationType:Value|Length:4");
        }

        /// <summary>
        /// The Class property with generation type value with null.
        /// </summary>
        [TestMethod]
        public void ClassPropertyWithGenerationTypeValueWithNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(v => v.Phone, o => o.GenerateFromValue(12345, 4).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "12345|Null:0.5|GenerationType:Value|Length:4");
        }

        /// <summary>
        /// The class name and property name and generat from list of date and null refernce has value.
        /// </summary>
        [TestMethod]
        public void ClassNameAndPropertyNameAndGeneratFromListOfDateAndNullRefernceHasValue()
        {
            // Arrange
            var dates = new List<DateTime> { new DateTime(2000, 5, 10), new DateTime(2005, 3, 22), new DateTime(2009, 8, 14), new DateTime(2010, 2, 1) };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(c => c.Date, o => o.GeneratFromList(dates).NullPercentage(0.7));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Date", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "5/10/2000 12:00:00 AM|3/22/2005 12:00:00 AM|8/14/2009 12:00:00 AM|2/1/2010 12:00:00 AM|Null:0.7|GenerationType:List");
        }

        /// <summary>
        /// The class name and property name and generat from list of string and null refernce has value.
        /// </summary>
        [TestMethod]
        public void ClassNameAndPropertyNameAndGeneratFromListOfStringAndNullRefernceHasValue()
        {
            // Arrange
            var emails = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(c => c.Email, o => o.GeneratFromList(emails).NullPercentage(0.7));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Email", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0.7|GenerationType:List");
        }

        /// <summary>
        /// The class name and property name and generat from list of string and null refernce not exist at all.
        /// </summary>
        [TestMethod]
        public void ClassNameAndPropertyNameAndGeneratFromListOfStringAndNullRefernceNotExistAtAll()
        {
            // Arrange
            var names = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<Customer>();
            repository.RuleUsesClassProperty(c => c.Name, o => o.GeneratFromList(names));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Customer|Name", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0|GenerationType:List");


            // Clean
        }

        /// <summary>
        /// The class name and property name and generat from random and null refernce has value.
        /// </summary>
        [TestMethod]
        public void ClassNameAndPropertyNameAndGeneratFromRandomAndNullRefernceHasValue()
        {
            // Arrange


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(c => c.Name, o => o.GenerateFromRandom<string>().NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Name", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0.5|GenerationType:Random");
        }

        /// <summary>
        /// The class name and property name and generat from range of date and null refernce has value.
        /// </summary>
        [TestMethod]
        public void ClassNameAndPropertyNameAndGeneratFromRangeOfDateAndNullRefernceHasValue()
        {
            // Arrange
            var fromDate = new DateTime(2005, 3, 22);
            var toDate = new DateTime(2010, 2, 1);


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(c => c.Date, o => o.GenerateFromRange(fromDate, toDate).NullPercentage(0.7));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Date", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "3/22/2005 12:00:00 AM|2/1/2010 12:00:00 AM|Null:0.7|GenerationType:Range");
        }

        /// <summary>
        /// The class name and property name and generat from range of int and null refernce has value.
        /// </summary>
        [TestMethod]
        public void ClassNameAndPropertyNameAndGeneratFromRangeOfIntAndNullRefernceHasValue()
        {
            // Arrange
            var fromInt = 1;
            var toInt = 100;


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesClassProperty(c => c.Phone, o => o.GenerateFromRange(fromInt, toInt).NullPercentage(0.6));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("VendorForm|Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|100|Null:0.6|GenerationType:Range");
        }

        /// <summary>
        /// The configure all settings.
        /// </summary>
        [TestMethod]
        public void ConfigureAllSettings()
        {
            // Arrange
            var repository = new FakeRepository();
            repository
                .Configure(o => o.SetAssemblyNameThatContainClasses("Radwan.dll"))
                .Configure(o => o.SetNamesapceThatContainClass("SeifNamespace"))
                .Configure(o => o.SetUseFakeableAttribute(true))
                .Configure(o => o.SetUseNotFakeableAttribute(true))
                .Configure(o => o.SetMaximumObjectGraphLevel(5000))
                .Configure(o => o.SetAssemblyPathThatContainClasses("C:/Radwan"))
                .Configure(o => o.SetCurrentRandomToDynamic());

            // Assert
            // remember this property internal and only available to the unit test assembly project
            Assert.IsTrue(frameworkSettings.EntitiesAssembly == "Radwan.dll");
            Assert.IsTrue(frameworkSettings.EntitiesNamespace == "SeifNamespace");
            Assert.IsTrue(frameworkSettings.UseFakeableAttribute);
            Assert.IsTrue(frameworkSettings.UseNotFakeableAttribute);
            Assert.IsTrue(frameworkSettings.MaximumObjectGraphLevel == 5000);
            Assert.IsTrue(frameworkSettings.CurrentRandom == frameworkSettings.RandomDynamicSeed);
            
        }

        /// <summary>
        /// Create(int numberOfObject) FakeRepository &lt;T&gt;--create list of object with nested object without saving
        ///   This test method test Create method that return list of object, it check that the method create list of object and generate it's data
        ///   this method also test for complex data generation, customer that has collection of order and collection of feedback, this method also test that the object didn't saved to the MemroryDb
        /// </summary>
        [TestMethod]
        public void CreateListOfObjectTest()
        {
            // Arrange
            var target = new FakeRepository<Customer>();

            // Act
            var list = target.Create(4);

            // Assert
            Assert.IsTrue(list.Count() == 4);
            Assert.IsNotNull(list.ToList()[0]);
            Assert.IsTrue((list.ToList()[0].Id > 0) && (!string.IsNullOrEmpty(list.ToList()[0].Name)) && (list.ToList()[0].Orders.Count == 4) && (list.ToList()[0].Feedbacks.Count == 4));
            Assert.IsTrue((list.ToList()[1].Id > 0) && (!string.IsNullOrEmpty(list.ToList()[1].Name)) && (list.ToList()[1].Orders.Count == 4) && (list.ToList()[1].Feedbacks.Count == 4));
            Assert.IsTrue((list.ToList()[2].Id > 0) && (!string.IsNullOrEmpty(list.ToList()[2].Name)) && (list.ToList()[2].Orders.Count == 4) && (list.ToList()[2].Feedbacks.Count == 4));
            Assert.IsTrue((list.ToList()[3].Id > 0) && (!string.IsNullOrEmpty(list.ToList()[3].Name)) && (list.ToList()[3].Orders.Count == 4) && (list.ToList()[3].Feedbacks.Count == 4));
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 0);

        }

        /// <summary>
        /// Create(int numberOfObject) FakeRepository &lt;T&gt;--create object with nested object without saving
        ///   This test method test Create method, it check that the method create object and generate it's data
        ///   this method also test for complex data generation, customer that has collection of order and collection of feedback, this method also test that the object didn't saved to the MemroryDb
        /// </summary>
        [TestMethod]
        public void CreateObjectTest()
        {
            // Arrange
            var target = new FakeRepository<Customer>();

            // Act
            var customer = target.Create();

            //// Assert
            Assert.IsNotNull(customer);
            Assert.IsTrue((customer.Id > 0) && (! string.IsNullOrEmpty(customer.Name)) && (customer.Orders.Count > 0) && (customer.Feedbacks.Count > 0));
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 0);

        }

        /// <summary>
        /// The data type with generation type list no null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeListNoNull()
        {
            // Arrange
            var nums = new List<int> { 1234, 5874, 8741, 6693 };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.Int, o => o.GeneratFromList(nums));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1234|5874|8741|6693|Null:0|GenerationType:List");
        }

        /// <summary>
        /// The data type with generation type list with null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeListWithNull()
        {
            // Arrange
            var names = new List<string> { "Radwan", "Seif", "Lara", "Rania" };

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.String, o => o.GeneratFromList(names).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("String", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0.5|GenerationType:List");
        }

        /// <summary>
        /// The data type with generation type random no null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeRandomNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.Int, o => o.GenerateFromRandom<int>());

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0|GenerationType:Random");
        }

        /// <summary>
        /// The data type with generation type random with null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeRandomWithNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.Int, o => o.GenerateFromRandom<int>().NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0.5|GenerationType:Random");
        }

        /// <summary>
        /// The data type with generation type range no null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeRangeNoNull()
        {
            // Arrange
            int from = 1;
            int to = 1000;

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d=>d.Int, o => o.GenerateFromRange(from, to));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|1000|Null:0|GenerationType:Range");
        }

        /// <summary>
        /// The data type with generation type range with null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeRangeWithNull()
        {
            // Arrange
            int from = 1;
            int to = 1000;

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.Int, o => o.GenerateFromRange(from, to).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|1000|Null:0.5|GenerationType:Range");
        }

        /// <summary>
        /// The data type with generation type value no null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeValueNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.Int, o => o.GenerateFromValue(12345, 4));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "12345|Null:0|GenerationType:Value|Length:4");
        }

        /// <summary>
        /// The data type with generation type value with null.
        /// </summary>
        [TestMethod]
        public void DataTypeWithGenerationTypeValueWithNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesDataType(d => d.Int, o => o.GenerateFromValue(12345, 4).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "12345|Null:0.5|GenerationType:Value|Length:4");
        }

        /// <summary>
        /// // Remove &lt;T&gt;(Expression &lt;Func&lt;T, bool&gt;&gt; where)
        ///   This method test delete an object by Id and not delete it's nested type if there nested types
        /// </summary>
        [TestMethod]
        public void DeleteManyObjectsByExpressionAndNotDeleteItsNested()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var vendor2 = new VendorForm { Code = "ven 2", Name = "vendor2" };
            var vendor3 = new VendorForm { Code = "ven 3", Name = "vendor3" };
            var target = new FakeRepository();
            target.Add(new List<VendorForm> { vendor1, vendor2, vendor3 });

            // Act
            target.Remove<VendorForm>(v => v.Id > 1);

            // Assert
            Assert.IsTrue(target.MemoryDb["M.Radwan.EntitiesTest.VendorForm"].Count == 1);
            Assert.IsTrue(target.MemoryDb["M.Radwan.EntitiesTest.VendorForm"][0].Code != "ven 2");
            Assert.IsTrue(target.MemoryDb["M.Radwan.EntitiesTest.VendorForm"][0].Code != "ven 3");
        }

        /// <summary>
        /// // Remove(T entity)
        ///   This method test delete an object by Id and not delete it's nested type if there nested types
        /// </summary>
        [TestMethod]
        public void DeleteObjectButNotDeleteItsNested()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var vendor2 = new VendorForm { Code = "ven 2", Name = "vendor2" };
            var vendor3 = new VendorForm { Code = "ven 3", Name = "vendor3" };
            var target = new FakeRepository();
            target.Add(new List<VendorForm> { vendor1, vendor2, vendor3 });

            // Act
            target.Remove(vendor2);

            // Assert
            Assert.IsTrue(target.MemoryDb["M.Radwan.EntitiesTest.VendorForm"].Count == 2);
            Assert.IsTrue(target.MemoryDb["M.Radwan.EntitiesTest.VendorForm"][0].Code != "ven 2");
            Assert.IsTrue(target.MemoryDb["M.Radwan.EntitiesTest.VendorForm"][1].Code != "ven 2");
        }

        /// <summary>
        /// The generate by data type for int data from list and null refernce has value.
        /// </summary>
        [TestMethod]
        public void GenerateByDataTypeForIntDataFromListAndNullRefernceHasValue()
        {
            // Arrange
            var nums = new List<int> { 1, 2, 3 };


            // Act
            var repository = new FakeRepository();
            repository.RuleUsesDataType(d => d.Int, o => o.GeneratFromList(nums).NullPercentage(0.3));
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            // Assert
            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|2|3|Null:0.3|GenerationType:List");
        }

        /// <summary>
        /// The generate by data type for int data from list and null refernce not exist at all.
        /// </summary>
        [TestMethod]
        public void GenerateByDataTypeForIntDataFromListAndNullRefernceNotExistAtAll()
        {
            // Arrange
            var nums = new List<int> { 1, 2, 3 };


            // Act
            var repository = new FakeRepository();
            repository.RuleUsesDataType(d => d.Int, o => o.GeneratFromList(nums));
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            // Assert
            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|2|3|Null:0|GenerationType:List");
        }

        /// <summary>
        /// The generate by data type for int data from value and null refernce has value.
        /// </summary>
        [TestMethod]
        public void GenerateByDataTypeForIntDataFromValueAndNullRefernceHasValue()
        {
            // Arrange
            int val = 123456789;


            // Act
            var repository = new FakeRepository();
            repository.RuleUsesDataType(d => d.Int, o => o.GenerateFromValue(val, 5).NullPercentage(0.3));
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Int32", out value);

            // Assert
            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "123456789|Null:0.3|GenerationType:Value|Length:5");
        }

        /// <summary>
        /// The generate by data type for string data from list and null refernce not exist at all.
        /// </summary>
        [TestMethod]
        public void GenerateByDataTypeForStringDataFromListAndNullRefernceNotExistAtAll()
        {
            // Arrange
            var list = new List<string> { "String 1", "String 2", "String 3", "String 1" };

            // Act
            var repository = new FakeRepository();
            repository.RuleUsesDataType(d => d.String, o => o.GeneratFromList(list));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("String", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "String 1|String 2|String 3|String 1|Null:0|GenerationType:List");

            // "String 1|String 2|String 3|String 1|Null:0|GenerationType:List  "
        }

        /// <summary>
        /// The generate by data type for string data from value and null refernce has value.
        /// </summary>
        [TestMethod]
        public void GenerateByDataTypeForStringDataFromValueAndNullRefernceHasValue()
        {
            // Arrange
            var stringValue = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


            // Act
            var repository = new FakeRepository();
            repository.RuleUsesDataType(d => d.String, o => o.GenerateFromValue(stringValue, 8).NullPercentage(0.3));
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("String", out value);

            // Assert
            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "ABCDEFGHIJKLMNOPQRSTUVWXYZ|Null:0.3|GenerationType:Value|Length:8");
        }

        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        // Use TestInitialize to run code before running each test

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- test counts of objects generated,
        ///   This test method will test the generate mechanism it assume that the class library that hold the Entities is "M.Radwan.EntitiesTest", it assume also that
        ///   we have 7 types (Customer,Order,VendorFrom,ServiceForm,ProductTypeForm, Orders, Location) the Orders is custom collection and the Location
        ///   is enumeration, because this version doesn't support them (custom collection and enumeration) so it this test method will test that they eliminated from the generated types
        ///   This test method also will test that all other types are generated with more than 3 objects
        /// </summary>
        [TestMethod]
        public void GenrateDataFor6CalessOf8ClassesInAssemblyRound3TimesAndObjectGrpahUnlimitedTest()
        {
            // Arrange
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> customeroutlist;
            List<dynamic> orderOutlist;
            List<dynamic> vendorFormOutlist;
            List<dynamic> serviceFormOutlist;
            List<dynamic> productTypeFormOutlist;
            List<dynamic> feedbackOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customeroutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ServiceForm", out serviceFormOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ProductTypeForm", out productTypeFormOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Feedback", out feedbackOutlist);

            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 7);
            Assert.IsTrue(customeroutlist.Count >= 3);
            Assert.IsTrue(orderOutlist.Count >= 3);
            Assert.IsTrue(vendorFormOutlist.Count >= 3);
            Assert.IsTrue(serviceFormOutlist.Count >= 3);
            Assert.IsTrue(productTypeFormOutlist.Count >= 3);
            Assert.IsTrue(feedbackOutlist.Count >= 3);
        }

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- test data is not empty, 
        ///   This test method will test the generate mechanism, it assume that the class library that hold the Entities is "M.Radwan.EntitiesTest", it assume also that
        ///   we have 3 types (Customer,Order,VendorFrom), it assume also that customer has list of order, so it test that customer will has orders and this order is the same object in the order key in
        ///   MemoryDb, it test also that some fields not null or empty and that's mean the generation was successfully
        /// </summary>
        [TestMethod]
        public void GenrateDataForAllCalessInAssemblyCheckComplextAndSimpleDataGenerationIsNotNullOrEmptyTest()
        {
            // Arrange
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> customeroutlist;
            List<dynamic> orderOutlist;
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customeroutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);


            Assert.IsTrue(customeroutlist[0].Name != string.Empty);
            Assert.IsTrue(customeroutlist[0].Name != null);
            Assert.IsTrue(customeroutlist[0].Orders.Count >= 1);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != string.Empty);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != null);

            Assert.IsTrue(orderOutlist[0].OrderName != string.Empty);
            Assert.IsTrue(orderOutlist[0].OrderName != null);
            Assert.IsTrue(orderOutlist[0].OrderName == customeroutlist[0].Orders[0].OrderName);

            Assert.IsTrue(vendorFormOutlist[0].Name != string.Empty);
            Assert.IsTrue(vendorFormOutlist[0].Name != null);

        }

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- Use NotFakeable Attribute-- test counts of objects generated,
        ///   This test method test that data generation only occur for classes that Not marked with NotFakeable attribute
        /// </summary>
        [TestMethod]
        public void GenrateDataForAllExceptNotFakeableAttributeAndCheckCountTest()
        {
            // Arrange
            // remember when I use NotFakeable with VendorForm, the vendorFrom still generated, this because it generated from the serviceForm because service form has VendorForm so the data generation generate data for all properties in serviceForm and the vendorFrom of course one of them, we may need to change this behavior in the future
            frameworkSettings.UseNotFakeableAttribute = true;
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(1);

            //// Assert
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 6);
        }

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- Use Fakeable Attribute-- test counts of objects generated,
        ///   This test method test that data generation only occur for classes that marked with Fakeable attribute
        /// </summary>
        [TestMethod]
        public void GenrateDataForFakeableAttributeOnlyAndCheckCountTest()
        {
            // Arrange
            frameworkSettings.UseFakeableAttribute = true;
            

            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> customeroutlist;
            List<dynamic> orderOutlist;
            List<dynamic> feedbackOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customeroutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Feedback", out feedbackOutlist);

            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 3);
            Assert.IsTrue(customeroutlist.Count >= 3);
            Assert.IsTrue(orderOutlist.Count >= 3);
            Assert.IsTrue(feedbackOutlist.Count >= 3);
        }

        /// <summary>
        /// The genrate data in assembly generation method based on value vendor email flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodBasedOnValueVendorEmailFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Email", "ABCDEFGHIJKLMNOPQRSTUVWXYZ|Null:0.3|GenerationType:Value|Length:8");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 3);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(((vendor.Email != null) ? vendor.Email.Length == 8 : true) || vendor.Email == "Seif@hotmail.com" || vendor.Email == null);
            }
        }

        /// <summary>
        /// The genrate data in assembly generation method list set list for vendor name nullis 1 listis 100 checkfor this list with flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodListSetListForVendorNameNullis1Listis100CheckforThisListWithFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            string listText;
            var sBuilder = new StringBuilder();

            for (int i = 0; i < 20; i++)
            {
                sBuilder.Append("Radwan|Seif|Lara|Rania|Hytham|");
            }

            listText = sBuilder.ToString();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Name", listText + "Null:0.1|GenerationType:List");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Name == "Radwan" || vendor.Name == "Seif" || vendor.Name == "Lara" || vendor.Name == "Rania" || vendor.Name == "Hytham" || vendor.Name == null);
            }
        }

        /// <summary>
        /// The genrate data in assembly generation method list set list for vendor name nullis 1 listis 2 checkfor this list with flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodListSetListForVendorNameNullis1Listis2CheckforThisListWithFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Name", "Radwan|Seif|Null:0.1|GenerationType:List");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Name == "Radwan" || vendor.Name == "Seif" || vendor.Name == null);
            }

        }

        /// <summary>
        /// The genrate data in assembly generation method list set list for vendor name nullis 3 listis 4 checkfor this list with flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodListSetListForVendorNameNullis3Listis4CheckforThisListWithFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Name", "Radwan|Seif|Lara|Rania|Null:0.3|GenerationType:List");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Name == "Radwan" || vendor.Name == "Seif" || vendor.Name == "Lara" || vendor.Name == "Rania" || vendor.Name == null);
            }

        }

        /// <summary>
        /// The genrate data in assembly generation method list set list for vendor name nullis 9 listis 100 checkfor this list with flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodListSetListForVendorNameNullis9Listis100CheckforThisListWithFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            string listText;
            var sBuilder = new StringBuilder();

            for (int i = 0; i < 20; i++)
            {
                sBuilder.Append("Radwan|Seif|Lara|Rania|Hytham|");
            }

            listText = sBuilder.ToString();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Name", listText + "Null:0.9|GenerationType:List");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Name == "Radwan" || vendor.Name == "Seif" || vendor.Name == "Lara" || vendor.Name == "Rania" || vendor.Name == "Hytham" || vendor.Name == null);
            }

        }

        /// <summary>
        /// The genrate data in assembly generation method list set list for vendor name nullis 9 listis 2 checkfor this list with flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodListSetListForVendorNameNullis9Listis2CheckforThisListWithFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Name", "Radwan|Seif|Null:0.9|GenerationType:List");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Name == "Radwan" || vendor.Name == "Seif" || vendor.Name == null);
            }

        }

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- test default generation data is done,
        ///   This test method will test the data generation mechanism, it assume that the class library that hold the Entities is "M.Radwan.EntitiesTest", it assume also that
        ///   we have 3 types (Customer,Order,VendorFrom), it assume also that customer has list of order, so it test that customer will has orders and each item in orders is the same object in the order value in MemeoryDb,
        ///   it test also that some fields not null or empty and that's mean the generation was successfully
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodNotSetCheckDefualtGenerationIsDoneTest()
        {
            // Arrange
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> customeroutlist;
            List<dynamic> orderOutlist;
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customeroutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);


            Assert.IsTrue(customeroutlist[0].Name != string.Empty);
            Assert.IsTrue(customeroutlist[0].Name != null);
            Assert.IsTrue(customeroutlist[0].Orders.Count >= 1);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != string.Empty);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != null);

            Assert.IsTrue(orderOutlist[0].OrderName != string.Empty);
            Assert.IsTrue(orderOutlist[0].OrderName != null);
            Assert.IsTrue(orderOutlist[0].OrderName == customeroutlist[0].Orders[0].OrderName);

            Assert.IsTrue(vendorFormOutlist[0].Name != string.Empty);
            Assert.IsTrue(vendorFormOutlist[0].Name != null);
        }

        /// <summary>
        /// The genrate data in assembly generation method random email property flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRandomEmailPropertyFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("Email", "Null:0.3|GenerationType:Random");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
           
        }

        /// <summary>
        /// The genrate data in assembly generation method random string data type and null zero flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRandomStringDataTypeAndNullZeroFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("String", "Null:0|GenerationType:Random");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Code != null);
                Assert.IsTrue(vendor.Name != null);
                Assert.IsTrue(vendor.Email != null);
                Assert.IsTrue(vendor.Address != null);
            }
        }

        /// <summary>
        /// The genrate data in assembly generation method random vendor date flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRandomVendorDateFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Date", "Null:0.3|GenerationType:Random");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
           
        }

        /// <summary>
        /// The genrate data in assembly generation method random vendor email flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRandomVendorEmailFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Email", "Null:0.3|GenerationType:Random");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            // Assert
            List<dynamic> vendorFormOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);
            Assert.IsTrue(vendorFormOutlist.Count >= 3);
        }

        /// <summary>
        /// The genrate data in assembly generation method randome vendor phone flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRandomeVendorPhoneFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Phone", "Null:0.3|GenerationType:Random");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
            
        }

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- test default generation data is done when there is no range define
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeButNoRangeValuesAreSetTest()
        {
            // Arrange
            frameworkSettings.EntitiesNamespace = "M.Radwan.EntitiesTest";
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> customeroutlist;
            List<dynamic> orderOutlist;
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customeroutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);


            Assert.IsTrue(customeroutlist[0].Name != string.Empty);
            Assert.IsTrue(customeroutlist[0].Name != null);
            Assert.IsTrue(customeroutlist[0].Orders.Count >= 1);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != string.Empty);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != null);

            Assert.IsTrue(orderOutlist[0].OrderName != string.Empty);
            Assert.IsTrue(orderOutlist[0].OrderName != null);
            Assert.IsTrue(orderOutlist[0].OrderName == customeroutlist[0].Orders[0].OrderName);

            Assert.IsTrue(vendorFormOutlist[0].Name != string.Empty);
            Assert.IsTrue(vendorFormOutlist[0].Name != null);
        }

        /// <summary>
        /// The genrate data in assembly generation method range email property flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeEmailPropertyFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("Email", "Radwan@DevMagicFake.com|Seif@hotmail.com|Null:0.3|GenerationType:Range");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Email == "Radwan@DevMagicFake.com" || vendor.Email == "Seif@hotmail.com" || vendor.Email == null);
            }


        }

        /// <summary>
        /// GenrateDataForAllAssemblyTypes(int numberOfRound) AbstractFakeRepository -- set Vendor Name, Vendor Email, Name, String, Int for other classes and check for them
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeSetRangeForVendorNameAndVendorEmailAndPropertyNameCheckforThsiRangeTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Name", "Sawsan|Rania|Radwan|Heba|Null:0|GenerationType:List");
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Email", "M.Radwan@gmail.com|Rania@hotmail.com|Lara@live.com|Null:0|GenerationType:List");
            frameworkSettings.DataGenerationPrimaryRules.Add("Name", "Seif|Lara|Haitham|Alaa|Nadia|Null:0|GenerationType:List");
            frameworkSettings.DataGenerationPrimaryRules.Add("Int32", "0123456789|Null:0|GenerationType:Value|Length:5");
            frameworkSettings.DataGenerationPrimaryRules.Add("String", "ABCDEFGHIJKLMNOPQRSTUVWXYZ|Null:0|GenerationType:Value|Length:8");
            

            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> customeroutlist;
            List<dynamic> orderOutlist;
            List<dynamic> vendorFormOutlist;
            List<dynamic> productTypeOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customeroutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ProductTypeForm", out productTypeOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 12);
            Assert.IsTrue(vendorFormOutlist[0].Name == "Sawsan" || vendorFormOutlist[0].Name == "Rania" || vendorFormOutlist[0].Name == "Radwan" || vendorFormOutlist[0].Name == "Heba");
            Assert.IsTrue(vendorFormOutlist[0].Email == "M.Radwan@gmail.com" || vendorFormOutlist[0].Email == "Rania@hotmail.com" || vendorFormOutlist[0].Email == "Lara@live.com");

            Assert.IsTrue(productTypeOutlist.Count >= 6);
            Assert.IsTrue(productTypeOutlist[0].Name == "Seif" || productTypeOutlist[0].Name == "Lara" || productTypeOutlist[0].Name == "Haitham" || productTypeOutlist[0].Name == "Alaa" || productTypeOutlist[0].Name == "Nadia");
            Assert.IsTrue(productTypeOutlist[0].Code.Length == 8);

            Assert.IsTrue(customeroutlist.Count >= 3);
            Assert.IsTrue(customeroutlist[0].Name == "Alaa" || customeroutlist[0].Name == "Haitham" || customeroutlist[0].Name == "Lara" || customeroutlist[0].Name == "Seif" || customeroutlist[0].Name == "Nadia");
            Assert.IsTrue(customeroutlist[0].Code.Length == 8);
            Assert.IsTrue(customeroutlist[0].Orders.Count >= 1);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != string.Empty);
            Assert.IsTrue(customeroutlist[0].Orders[0].OrderName != null);

            Assert.IsTrue(orderOutlist[0].OrderName != string.Empty);
            Assert.IsTrue(orderOutlist[0].OrderName != null);
            Assert.IsTrue(orderOutlist[0].OrderName == customeroutlist[0].Orders[0].OrderName);
        }

        /// <summary>
        /// The genrate data in assembly generation method range string data type flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeStringDataTypeFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("String", "String 1|String 1|Null:0.3|GenerationType:Range");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Code == "String 2" || vendor.Code == "String 1" || vendor.Code == null);
                Assert.IsTrue(vendor.Name == "String 2" || vendor.Name == "String 1" || vendor.Name == null);
                Assert.IsTrue(vendor.Email == "String 2" || vendor.Email == "String 1" || vendor.Email == null);
                Assert.IsTrue(vendor.Address == "String 2" || vendor.Address == "String 1" || vendor.Address == null);
            }
        }

        /// <summary>
        /// The genrate data in assembly generation method range vendor date flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeVendorDateFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Date", "1/1/2005|1/1/2010|Null:0.3|GenerationType:Range");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Date >= new DateTime(2005, 1, 1) || vendor.Date == DateTime.Parse("1/1/0001 12:00:00 AM"));
                Assert.IsTrue(vendor.Date <= new DateTime(2010, 1, 1) || vendor.Date == DateTime.Parse("1/1/0001 12:00:00 AM"));
            }


        }

        /* Test Generate from Range based on DataGenerationPrimaryRules That populated by Expression tree feature
       * =======================================================================================
       */

        /// <summary>
        /// The genrate data in assembly generation method range vendor email flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeVendorEmailFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Email", "Radwan@DevMagicFake.com|Seif@hotmail.com|Null:0.3|GenerationType:Range");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 3);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Email == "Radwan@DevMagicFake.com" || vendor.Email == "Seif@hotmail.com" || vendor.Email == null);
            }

        }

        /// <summary>
        /// The genrate data in assembly generation method range vendor phone flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodRangeVendorPhoneFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Phone", "1|100|Null:0.3|GenerationType:Range");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 5);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(vendor.Phone <= 100);
            }
        }

        /// <summary>
        /// The genrate data in assembly generation method value expect expception because length vendor date flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodValueExpectExpceptionBecauseLengthVendorDateFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Email", "ABCDEFGHIJKLMNOPQRSTUVWXYZ|Null:0.3|GenerationType:Value|Length:200");
            var target = new FakeRepository();

            // Act
            try
            {
                target.GenerateDataForAllAssemblyTypes(5);
            }
            catch (Exception e)
            {
                if (e.InnerException.Message != "Max length must be between 1 and 100")
                {
                    Assert.Fail("Exception of wrong length didn't thrown");
                }
            }


        }

        /// <summary>
        /// The genrate data in assembly generation method value vendor phone flunt config test.
        /// </summary>
        [TestMethod]
        public void GenrateDataInAssemblyGenerationMethodValueVendorPhoneFluntConfigTest()
        {
            // Arrange
            frameworkSettings.DataGenerationPrimaryRules.Clear();
            frameworkSettings.DataGenerationPrimaryRules.Add("VendorForm|Phone", "0123456789|100|Null:0.3|GenerationType:Value|Length:5");
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(5);

            //// Assert
            List<dynamic> vendorFormOutlist;

            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorFormOutlist);

            Assert.IsTrue(vendorFormOutlist.Count >= 20);
            foreach (var vendor in vendorFormOutlist)
            {
                Assert.IsTrue(((vendor.Phone != 0) ? vendor.Phone.ToString().Length <= 5 : true) || vendor.Phone == 0);
            }


        }

        /// <summary>
        /// // GetMany &lt;T&gt;(Expression &lt;Func&lt;T, bool&gt;&gt; where)
        ///   This method test retrieve a list of objects like VendorFrom using expression tree of func
        /// </summary>
        [TestMethod]
        public void GetManytOfObjectsUsingExpressionTree()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var vendor2 = new VendorForm { Code = "ven 2", Name = "vendor2" };
            var vendor3 = new VendorForm { Code = "ven 3", Name = "vendor3" };
            var target = new FakeRepository();
            target.Add(new List<VendorForm> { vendor1, vendor2, vendor3 });

            // Act
            var vendorList = target.GetMany<VendorForm>(v => v.Id > 1).ToList();

            // Assert
            Assert.IsTrue(vendorList.Count() == 2);
            Assert.IsTrue(vendorList.ToList()[0].Code == "ven 2");
            Assert.IsTrue(vendorList.ToList()[1].Code == "ven 3");
        }

        /// <summary>
        /// // GetObjectByCode
        ///   This method test retrieve single object like VendorFrom using
        /// </summary>
        [TestMethod]
        public void GetObjectByCode()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var vendor2 = new VendorForm { Code = "ven 2", Name = "vendor2" };
            var vendor3 = new VendorForm { Code = "ven 3", Name = "vendor3" };
            var target = new FakeRepository();
            target.Add(new List<VendorForm> { vendor1, vendor2, vendor3 });

            // Act
            var vendor = target.GetByCode<VendorForm>("ven 2");

            // Assert
            Assert.IsTrue(vendor != null);
            Assert.IsTrue(vendor.Id == 2);
        }

        /// <summary>
        /// // Get &lt;T&gt;()
        ///   This method test retrieve single object like VendorFrom using expression tree of func
        /// </summary>
        [TestMethod]
        public void GetObjectUsingExpressionTree()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var vendor2 = new VendorForm { Code = "ven 2", Name = "vendor2" };
            var vendor3 = new VendorForm { Code = "ven 3", Name = "vendor3" };
            var target = new FakeRepository();
            target.Add(new List<VendorForm> { vendor1, vendor2, vendor3 });

            // Act
            var vendor = target.Get<VendorForm>(v => v.Name == "vendor2");

            // Assert
            Assert.IsTrue(vendor.Code == "ven 2");
        }

        /// <summary>
        /// The property name and generat from list of integer and null refernce has value.
        /// </summary>
        [TestMethod]
        public void PropertyNameAndGeneratFromListOfIntegerAndNullRefernceHasValue()
        {
            // Arrange
            var nums = new List<int> { 1, 5, 20, 254 };


            // Act
            var repository = new FakeRepository<Customer>();
            repository.RuleUsesPropertyOnly(p => "Id", o => o.GeneratFromList(nums).NullPercentage(0.2));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Id", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|5|20|254|Null:0.2|GenerationType:List");

        }

        /// <summary>
        /// The property name and generat from list of integer and null refernce not exist at all.
        /// </summary>
        [TestMethod]
        public void PropertyNameAndGeneratFromListOfIntegerAndNullRefernceNotExistAtAll()
        {
            // Arrange
            var nums = new List<int> { 1, 5, 20, 254 };


            // Act
            FakeRepository<Customer> repository = new FakeRepository<Customer>();
            repository.RuleUsesPropertyOnly(p => "Id", o => o.GeneratFromList(nums));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Id", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|5|20|254|Null:0|GenerationType:List");

        }

        /// <summary>
        /// The property name and generat from list of string and null refernce has value.
        /// </summary>
        [TestMethod]
        public void PropertyNameAndGeneratFromListOfStringAndNullRefernceHasValue()
        {
            // Arrange
            var emails = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<Customer>();
            repository.RuleUsesPropertyOnly(p => "Email", o => o.GeneratFromList(emails).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Email", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0.5|GenerationType:List");

        }

        /// <summary>
        /// The property name and generat from list of string and null refernce not exist at all.
        /// </summary>
        [TestMethod]
        public void PropertyNameAndGeneratFromListOfStringAndNullRefernceNotExistAtAll()
        {
            // Arrange
            var emails = new List<string> { "Radwan", "Seif", "Lara", "Rania" };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(p => "Email", o => o.GeneratFromList(emails));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Email", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0|GenerationType:List");
        }

        /// <summary>
        /// The property only with generation type list no null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeListNoNull()
        {
            // Arrange
            var nums = new List<int> { 1234, 5874, 8741, 6693 };


            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Phone", o => o.GeneratFromList(nums));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1234|5874|8741|6693|Null:0|GenerationType:List");
        }

        /// <summary>
        /// The property only with generation type list with null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeListWithNull()
        {
            // Arrange
            var names = new List<string> { "Radwan", "Seif", "Lara", "Rania" };

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Name", o => o.GeneratFromList(names).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Name", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Radwan|Seif|Lara|Rania|Null:0.5|GenerationType:List");
        }

        /// <summary>
        /// The property only with generation type random no null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeRandomNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Phone", o => o.GenerateFromRandom<int>());

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0|GenerationType:Random");
        }

        /// <summary>
        /// The property only with generation type random with parameter no null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeRandomWithParameterNoNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Phone", o => o.GenerateFromRandom(d => d.Int));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0|GenerationType:Random");
        }

        /// <summary>
        /// The property only with generation type random with null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeRandomWithNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Phone", o => o.GenerateFromRandom<int>().NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "Null:0.5|GenerationType:Random");
        }

        /// <summary>
        /// The property only with generation type range no null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeRangeNoNull()
        {
            // Arrange
            string from = new DateTime(2000, 1, 1).ToString();
            string to = new DateTime(2010, 1, 1).ToString();

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Date", o => o.GenerateFromRange(from, to));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Date", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1/1/2000 12:00:00 AM|1/1/2010 12:00:00 AM|Null:0|GenerationType:Range");
        }

        /// <summary>
        /// The property only with generation type range with null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeRangeWithNull()
        {
            // Arrange
            int from = 1;
            int to = 1000;

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d => "Code", o => o.GenerateFromRange(from, to).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Code", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "1|1000|Null:0.5|GenerationType:Range");
        }

        /// <summary>
        /// The property only with generation type value no null 1.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeValueNoNull1()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d =>"Phone", o => o.GenerateFromValue(12345, 4));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Phone", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "12345|Null:0|GenerationType:Value|Length:4");
        }

        /// <summary>
        /// The property only with generation type value with null.
        /// </summary>
        [TestMethod]
        public void PropertyOnlyWithGenerationTypeValueWithNull()
        {
            // Arrange

            // Act
            var repository = new FakeRepository<VendorForm>();
            repository.RuleUsesPropertyOnly(d =>"Id", o => o.GenerateFromValue(12345, 4).NullPercentage(0.5));

            // Assert
            // remember this property internal and only available to the unit test assembly project
            string value;
            var hasValue = frameworkSettings.DataGenerationPrimaryRules.TryGetValue("Id", out value);

            Assert.IsTrue(hasValue);
            Assert.IsTrue(value == "12345|Null:0.5|GenerationType:Value|Length:4");
        }

        /// <summary>
        /// Using LINQ to retrieve all customers
        ///   This test method test the ability of query the MemroryDb using LINQ, it retrieve all customers
        /// </summary>
        [TestMethod]
        public void QueryMemoryDbUsingLinqTest()
        {
            // Arrange
            var target = new FakeRepository();

            // Act
            target.GenerateDataForAllAssemblyTypes(3);

            //// Assert
            var customers = from c in target.MemoryDb["M.Radwan.EntitiesTest.Customer"]
                            select c;
            Assert.IsTrue(customers.ToList().Count > 0);
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X&gt; ---&gt; X is collection --Save Nested collection item, 
        ///   This method test the ability of the framework to save object that has generic collection like List or IList or IEnumrable of items
        ///   for example Customer that has collection of Order, the test method assume that the customer has all information for each order in the orders, so the test method test for save the main object and and save each item in the nested collection
        ///   so if any id of any order in nested collection in the customer exist in the MemoeryDB it will be overwritten by the new one
        ///   if there is a case that we want to save the customer including the order collection we will use the SaveAll method that save with nested type
        ///   This method depend on creating FakeRepository&lt;Customer, List&lt;Order&gt;&gt; that's mean create fake object of the main object and the list of item as nested type
        /// </summary>
        [TestMethod]
        public void SaveAllForOneObjectThatHasOneCollectionAndSaveAllItsItemInMemoryDbTest()
        {
            // Arrange 1
            var order11 = new Order { Id = 1, OrderName = "Order 1" };
            var order12 = new Order { Id = 2, OrderName = "Order 2" };
            var order13 = new Order { Id = 3, OrderName = "Order 3" };
            var orders = new List<Order> { order11, order12, order13 };
            var fakeRepository = new FakeRepository<Order>();
            fakeRepository.Add(orders);


            // Arrange 2
            var customer = new Customer { Code = "cus 1,", Name = "Customer 1" };

            var order1 = new Order { Id = 1, Code = "code 1", OrderName = "This order 1" };
            var order2 = new Order { Id = 2, Code = "code 2", OrderName = "This order 2" };
            var order3 = new Order { Id = 3, Code = "code 3", OrderName = "This order 3" };
            var customerOrders = new List<Order> { order1, order2, order3 };
            customer.Orders = customerOrders;


            var target = new FakeRepository<Customer, List<Order>>();

            // Act
            target.AddAll(customer);

            //// Assert
            List<dynamic> outlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out outlist);
            Assert.IsTrue(outlist[0].OrderName != "Order 1");
            Assert.IsTrue(outlist[1].OrderName != "Order 2");
            Assert.IsTrue(outlist[2].OrderName != "Order 3");

            Assert.IsTrue(outlist[0].OrderName == "This order 1");
            Assert.IsTrue(outlist[1].OrderName == "This order 2");
            Assert.IsTrue(outlist[2].OrderName == "This order 3");
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X,y&gt; ---&gt; X is collection, Y is collection --Save Nested collection Item,
        ///   This method test the ability of the framework to save object that has generic collection like List or IList or IEnumrable of items
        ///   for example Customer that has collection of Order and a collection of Feedback, the test method assume that the customer has all information for each order in the orders and all information for each Feedback in Feedbacks, so the test method test for save the main object and and save each item in the nested collections
        ///   so if any id of any order or feedback in the nested collection in the customer exist in the MemoeryDB it will be overwritten by the new one
        ///   if there is a case that we want to save the customer including the order collection we will use the SaveAll method that save with nested type
        ///   This method depend on creating FakeRepository&lt;Customer, List&lt;Order&gt;,List&lt;Feedback&gt;&gt; that's mean create fake object of the main object and the 2 lists of item as nested types
        /// </summary>
        [TestMethod]
        public void SaveAllForOneObjectThatHasTwoCollectionAndSaveAllItsItemInMemoryDbTest()
        {
            // Arrange 1
            var order11 = new Order { Id = 1, OrderName = "Order 1" };
            var order12 = new Order { Id = 2, OrderName = "Order 2" };
            var order13 = new Order { Id = 3, OrderName = "Order 3" };
            var orders = new List<Order> { order11, order12, order13 };
            var fakeRepository1 = new FakeRepository<Order>();
            fakeRepository1.Add(orders);

            var feeback11 = new Feedback { Id = 1, Description = "Description 1" };
            var feeback12 = new Feedback { Id = 2, Description = "Description 2" };
            var feeback13 = new Feedback { Id = 3, Description = "Description 3" };
            var feedbacks = new List<Feedback> { feeback11, feeback12, feeback13 };
            var fakeRepository2 = new FakeRepository<Feedback>();
            fakeRepository2.Add(feedbacks);

            // Arrange 2
            var customer = new Customer { Code = "cus 1,", Name = "Customer 1" };

            var order1 = new Order { Id = 1, Code = "code 1", OrderName = "This order 1" };
            var order2 = new Order { Id = 2, Code = "code 2", OrderName = "This order 2" };
            var order3 = new Order { Id = 3, Code = "code 3", OrderName = "This order 3" };
            var customerOrders = new List<Order> { order1, order2, order3 };

            var feeback1 = new Feedback { Id = 1, Description = "new Description 1" };
            var feeback2 = new Feedback { Id = 2, Description = "new Description 2" };
            var feeback3 = new Feedback { Id = 3, Description = "new Description 3" };
            var customerFeedbacks = new List<Feedback> { feeback1, feeback2, feeback3 };


            customer.Orders = customerOrders;
            customer.Feedbacks = customerFeedbacks;

            var target = new FakeRepository<Customer, List<Order>, List<Feedback>>();

            // Act
            target.AddAll(customer);

            //// Assert
            List<dynamic> orderOutlist;
            List<dynamic> feedbackOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Order", out orderOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Feedback", out feedbackOutlist);
            Assert.IsTrue(orderOutlist[0].OrderName != "Order 1");
            Assert.IsTrue(orderOutlist[1].OrderName != "Order 2");
            Assert.IsTrue(orderOutlist[2].OrderName != "Order 3");

            Assert.IsTrue(orderOutlist[0].OrderName == "This order 1");
            Assert.IsTrue(orderOutlist[1].OrderName == "This order 2");
            Assert.IsTrue(orderOutlist[2].OrderName == "This order 3");

            Assert.IsTrue(feedbackOutlist[0].Description != "Description 1");
            Assert.IsTrue(feedbackOutlist[1].Description != "Description 2");
            Assert.IsTrue(feedbackOutlist[2].Description != "Description 3");

            Assert.IsTrue(feedbackOutlist[0].Description == "new Description 1");
            Assert.IsTrue(feedbackOutlist[1].Description == "new Description 2");
            Assert.IsTrue(feedbackOutlist[2].Description == "new Description 3");
        }

        /// <summary>
        /// SaveAll(T obj) FakeRepository &lt;T,X&gt;  ---&gt; X is single object --Save Nested Item,
        ///   Test SaveAll for FakeRepository &lt;T,X &gt; this test method save the main object and the nested object, so it assume that we want to save both object (main, nested) so we will not retrieve the nested object from the MemoryDb by Id and link it to the main object like Save method but we will save both (main, nested), so if there are data in the nested in the
        ///   MemoryDb it will be updated by the new one
        /// </summary>
        [TestMethod]
        public void SaveAllForSingleComplextObjectHasOneObjectsAndSaveNestedObjectToMemoryDbWihoutRetriveByIdFromMemoryDbTest()
        {
            // Arrange
            var vendorPreTest = new VendorForm { Code = "ven 1", Name = "Old vendor", Address = "Address 1", Email = "Email 1" };
            var fakeRepository = new FakeRepository<VendorForm>();
            fakeRepository.Add(vendorPreTest);

            var vendor = new VendorForm { Id = 1, Code = "new ven 1", Name = "New vendor", Address = "new Address 1", Email = "New Email 1" };
            var productTypeForm = new ProductTypeForm { Id = 1, Name = "ProductTypeForm 1", VendorForm = vendor };
            var target = new FakeRepository<ProductTypeForm, VendorForm>();

            // Act
            target.AddAll(productTypeForm);

            // Assert
            List<dynamic> productTypeoutlist;
            List<dynamic> vendorOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ProductTypeForm", out productTypeoutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorOutlist);
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Email == "New Email 1");
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Address == vendorOutlist[0].Address);
        }

        /// <summary>
        /// SaveAll(T obj) FakeRepository&lt;T,X,Y &gt; ---&gt; X is single object,  ---&gt; Y is single object  --Save Nested Item, 
        ///   Test SaveAll for FakeRepository &lt;T,X,Y &gt; this test method save the main object and the nested objects, so it assume that we want to save all objects (main, first nested, second nested) so we will not retrieve the nested object from the MemoryDb by Id and link it to the main object, this method will save both (main, nested, nested), so if there is data in the nested at the
        ///   MemoryDb it will be updated by the new one
        /// </summary>
        [TestMethod]
        public void SaveAllForSingleComplextObjectHasTwoObjectsAndSaveNestedObjectToMemoryDbWihoutRetriveByIdFromMemoryDbTest()
        {
            // Arrange
            var vendorPreTest = new VendorForm { Code = "ven 1", Name = "Old vendor", Address = "Address 1", Email = "Email 1" };
            var vendorFakeRepository = new FakeRepository<VendorForm>();
            vendorFakeRepository.Add(vendorPreTest);

            var productTypePreTest = new ProductTypeForm { Code = "prod 1", Name = "Old productType", VendorForm = vendorPreTest };
            var productFakeRepository = new FakeRepository<ProductTypeForm, VendorForm>();
            productFakeRepository.Add(productTypePreTest);

            var vendor = new VendorForm { Id = 1, Code = "new ven 1", Name = "New vendor", Address = "new Address 1", Email = "New Email 1" };
            var productTypeForm = new ProductTypeForm { Id = 1, Name = "new ProductTypeForm 1", VendorForm = vendor };
            var service = new ServiceForm { Code = "serv 1", Name = "service 1", Description = "Description 1", FixedCommission = 33, Price = 4, ProductTypeForm = productTypeForm, VendorForm = vendor };
            var target = new FakeRepository<ServiceForm, ProductTypeForm, VendorForm>();

            // Act
            target.AddAll(service);

            // Assert
            List<dynamic> productTypeoutlist;
            List<dynamic> vendorOutlist;
            List<dynamic> serviceOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ProductTypeForm", out productTypeoutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorOutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ServiceForm", out serviceOutlist);
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 3);
            Assert.IsTrue(productTypeoutlist.Count == 1);
            Assert.IsTrue(vendorOutlist.Count == 1);
            Assert.IsTrue(serviceOutlist.Count == 1);
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Email == "New Email 1");
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Address == vendorOutlist[0].Address);
            Assert.IsTrue(serviceOutlist[0].ProductTypeForm.Name == "new ProductTypeForm 1");
            Assert.IsTrue(serviceOutlist[0].ProductTypeForm.Name == productTypeoutlist[0].Name);
            Assert.IsTrue(serviceOutlist[0].VendorForm.Name == vendorOutlist[0].Name);
        }

        /// <summary>
        /// Save(List &lt;T&gt; list) FakeRepository &lt;T&gt;, 
        ///   This method test save a list of single object VendorFrom this object doesn't include any complex type just .NET type
        /// </summary>
        [TestMethod]
        public void SaveListOfSimpleObject()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var vendor2 = new VendorForm { Code = "ven 2", Name = "vendor2" };
            var vendor3 = new VendorForm { Code = "ven 3", Name = "vendor3" };
            var vendor4 = new VendorForm { Code = "ven 4", Name = "vendor4" };

            var vendors = new List<VendorForm> { vendor1, vendor2, vendor3, vendor4 };

            var target = new FakeRepository<VendorForm>();

            // Act
            target.Add(vendors);

            // Assert
            List<dynamic> outlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out outlist);
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 1);
            Assert.IsTrue(outlist.Count == 4);
            Assert.IsTrue(outlist[0].Code == "ven 1");
            Assert.IsTrue(outlist[0].Name == "vendor1");
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X&gt; ---&gt; X is collection --Retrieve Nested collection Item, 
        ///   This method test the ability of the framework to save object that has generic collection like List or IList or IEnumrable of items
        ///   for example Customer that has collection of Order, the test method assume that the customer only has collection of order with id for each one, so the test method test for save the main object and retrieve each item from MemeryDB and add it to the main object customer orders property
        ///   so the id of the order in the created customer must be exist in the MemoeryDB otherwise it will empty the collection in the customer
        ///   if there is a case that we want to save the customer including the order collection we will use the SaveAll method that save with nested type
        ///   This method depend on creating FakeRepository&lt;Customer, List&lt;Order&gt;&gt; that's mean create fake object of the main object and the list of item as nested type
        /// </summary>
        [TestMethod]
        public void SaveObjectThatHasOneCollectionAndRetriveItsItemFromMemoryDbByIdAndDiscardCollectionItemCurrentDataTest()
        {
            // Arrange 1
            var order11 = new Order { Id = 1, OrderName = "Order 1" };
            var order12 = new Order { Id = 2, OrderName = "Order 2" };
            var order13 = new Order { Id = 3, OrderName = "Order 3" };
            var orders = new List<Order> { order11, order12, order13 };
            var fakeRepository = new FakeRepository<Order>();
            fakeRepository.Add(orders);


            // Arrange 2
            var customer = new Customer { Code = "cus 1,", Name = "Customer 1" };

            var order1 = new Order { Id = 1, Code = "code 1" };
            var order2 = new Order { Id = 2, Code = "code 2" };
            var order3 = new Order { Id = 3, Code = "code 3" };
            var customerOrders = new List<Order> { order1, order2, order3 };
            customer.Orders = customerOrders;


            var target = new FakeRepository<Customer, List<Order>>();

            // Act
            target.Add(customer);

            //// Assert
            List<dynamic> outlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out outlist);
            Assert.IsTrue(outlist[0].Orders[0].OrderName == "Order 1");
            Assert.IsTrue(outlist[0].Orders[1].OrderName == "Order 2");
            Assert.IsTrue(outlist[0].Orders[2].OrderName == "Order 3");
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X,y&gt; ---&gt; X is collection Y is collection --Retrieve Nested collection Item
        ///   This method test the ability of the framework to save object that has generic collection like List or IList or IEnumrable of items
        ///   for example Customer that has collection of Order and collection of feedback, the test method assume that the customer has a collection of order with id for each one, and a collection of feedback with id for each one so the test method test for save the main object and retrieve each item from MemeryDB and add it to the main object customer orders and feedbacks properties
        ///   so the id of the order or the feedback in the created customer must be exist in the MemoeryDB otherwise it will empty the collection in the customer
        ///   if there is a case that we want to save the customer including the order collection and the feedback collection we will use the SaveAll method that save with nested type
        ///   This method depend on creating FakeRepository&lt;Customer, List&lt;Order&gt;,List&lt;Feedback&gt;&gt; that's mean create fake object of the main object and the 2 lists of item as nested types
        /// </summary>
        [TestMethod]
        public void SaveObjectThatHasTwoCollectionAndRetriveItsItemFromMemoryDbByIdAndDiscardCollectionItemCurrentDataTest()
        {
            // Arrange 1
            var order11 = new Order { Id = 1, OrderName = "Order 1" };
            var order12 = new Order { Id = 2, OrderName = "Order 2" };
            var order13 = new Order { Id = 3, OrderName = "Order 3" };
            var orders = new List<Order> { order11, order12, order13 };
            var fakeRepository1 = new FakeRepository<Order>();
            fakeRepository1.Add(orders);

            var feeback11 = new Feedback { Id = 1, Description = "Description 1" };
            var feeback12 = new Feedback { Id = 2, Description = "Description 2" };
            var feeback13 = new Feedback { Id = 3, Description = "Description 3" };
            var feedbacks = new List<Feedback> { feeback11, feeback12, feeback13 };
            var fakeRepository2 = new FakeRepository<Feedback>();
            fakeRepository2.Add(feedbacks);


            // Arrange 2
            var customer = new Customer { Code = "cus 1,", Name = "Customer 1" };

            var order1 = new Order { Id = 1, Code = "code 1" };
            var order2 = new Order { Id = 2, Code = "code 2" };
            var order3 = new Order { Id = 3, Code = "code 3" };
            var customerOrders = new List<Order> { order1, order2, order3 };

            var feeback1 = new Feedback { Id = 1, Description = "new Description" };
            var feeback2 = new Feedback { Id = 2, Description = "new Description" };
            var feeback3 = new Feedback { Id = 3, Description = "new Description" };
            var customerFeedbacks = new List<Feedback> { feeback1, feeback2, feeback3 };


            customer.Orders = customerOrders;
            customer.Feedbacks = customerFeedbacks;

            var target = new FakeRepository<Customer, List<Order>, List<Feedback>>();

            // Act
            target.Add(customer);

            //// Assert
            List<dynamic> customerOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.Customer", out customerOutlist);
            Assert.IsTrue(customerOutlist[0].Orders[0].OrderName == "Order 1");
            Assert.IsTrue(customerOutlist[0].Orders[1].OrderName == "Order 2");
            Assert.IsTrue(customerOutlist[0].Orders[2].OrderName == "Order 3");

            Assert.IsTrue(customerOutlist[0].Feedbacks[0].Description == "Description 1");
            Assert.IsTrue(customerOutlist[0].Feedbacks[1].Description == "Description 2");
            Assert.IsTrue(customerOutlist[0].Feedbacks[2].Description == "Description 3");
        }

        /// <summary>
        /// // Save(T obj) FakeRepository &lt;T&gt;
        ///   This method test save single object like VendorFrom this object doesn't include any complex type, just .NET types
        /// </summary>
        [TestMethod]
        public void SaveSingleAndSimpleObject()
        {
            // Arrange
            var vendor1 = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var target = new FakeRepository<VendorForm>();

            // Act
            target.Add(vendor1);

            // Assert
            List<dynamic> outlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out outlist);
            Assert.IsTrue(outlist.Count == 1);
            Assert.IsTrue(outlist[0].Code == "ven 1");
            Assert.IsTrue(outlist[0].Name == "vendor1");
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T&gt; --- no Id property,
        ///   This method test to save single object like VendorFrom, this object doesn't include any complex type, just .NET types
        /// </summary>
        [TestMethod]
        public void SaveSingleAndSimpleObjectThatDoesntHasIdPropertyTest()
        {
            // Arrange
            var student = new Student { Code = "stu 1", Name = "student 1" };
            var target = new FakeRepository<Student>();

            // Act
            target.Add(student);

            // Assert
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 0);
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X&gt; ---&gt; X is single object --Retrieve Nested Item, 
        ///   Test Save for FakeRepository &lt;T,X &gt; this test method test the method like when we save the productType in GUI we just select the vendor from drop down list that provide this product, we entered the data of the product and save it, 
        ///   remember that when we select the vendor from the drop down menu this vendor already exist in our system and we only have the id and the name of the vendor at this GUI form,
        ///   The main purpose of the method is to save the product under the chosen vendor and of course the vendor is existing in our system so we will not save the vendor we just need to link
        ///   the new created product to the chosen existing vendor
        /// </summary>
        [TestMethod]
        public void SaveSingleComplextObjectHasOneObjectsThatRetriveItsDataByIdFromMemoryDbAndDiscardItsCurrentDataTest()
        {
            // Arrange
            var vendorPreTest = new VendorForm { Code = "ven 1", Name = "vendor 1", Address = "Address 1", Email = "Email 1" };
            var fakeRepository = new FakeRepository<VendorForm>();
            fakeRepository.Add(vendorPreTest);

            var vendor = new VendorForm { Id = 1, Name = "vendor1" };
            var productTypeForm = new ProductTypeForm { Name = "ProductTypeForm 1", VendorForm = vendor };
            var target = new FakeRepository<ProductTypeForm, VendorForm>();

            // Act
            target.Add(productTypeForm);

            // Assert
            List<dynamic> productTypeoutlist;
            List<dynamic> vendorOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ProductTypeForm", out productTypeoutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorOutlist);
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Email == "Email 1");
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Email == vendorOutlist[0].Email);
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X,Y &gt; ---&gt; X is single object,  ---&gt; Y is single object  --Retrieve Nested Item, 
        ///   This method test the ability of the framework to save object that has generic collection like List or IList or IEnumrable of items
        ///   for example create Service in the GUI so we just select the vendor from drop down list and this vendor has many products so we select product from drop down list, we enter the data of the service and save
        ///   remember when we select the vendor from the drop down menu we just have the id and the name for the vendor, and also the retrieved product we only have the Id and name that appear in the drop down list, remember also that we don't want to save both vendor neither the product because they already saved
        ///   if there is any case that we want to save the the vendor and the product with the service we will use SaveAll method that save object with nested.
        /// </summary>
        [TestMethod]
        public void SaveSingleComplextObjectHasTwoObjectsThatRetriveItsDataByIdFromMemoryDbAndDiscardItsCurrentDataTest()
        {
            // Arrange
            var vendorPreSave = new VendorForm { Id = 1, Code = "ven1", Name = "vendor1", Address = "Address1", Date = DateTime.Now, Email = "Email1" };
            var productTypePreSave = new ProductTypeForm { Id = 1, Name = "ProductType1", Code = "Prod1", VendorForm = vendorPreSave };
            var vendorFakeRepository = new FakeRepository<VendorForm>();
            var productFakeRepository = new FakeRepository<ProductTypeForm, VendorForm>();
            vendorFakeRepository.Add(vendorPreSave);
            productFakeRepository.Add(productTypePreSave);

            // Arrange
            var vendor = new VendorForm { Id = 1, Name = "vendor1" };
            var productType = new ProductTypeForm { Id = 1, Name = "ProductType1", VendorForm = vendor };

            var service = new ServiceForm { Code = "Ser1", Name = "Service1", Price = (decimal)11.3, FixedCommission = 20, ProductTypeForm = productType, VendorForm = vendor };

            var target = new FakeRepository<ServiceForm, VendorForm, ProductTypeForm>();
            target.Add(service);


            // Assert
            List<dynamic> outlist;
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 3);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ServiceForm", out outlist);
            Assert.IsTrue(outlist[0].VendorForm.Address == "Address1");
            Assert.IsTrue(outlist[0].ProductTypeForm.Code == "Prod1");
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T&gt;--Update,
        ///   This method test update single object like VendorFrom, this object doesn't include any complex type, just .NET types
        /// </summary>
        [TestMethod]
        public void SaveUpdateSingleAndSimpleObject()
        {
            // Arrange
            var vendorFirstSave = new VendorForm { Code = "ven 1", Name = "vendor1" };
            var fakeRepository = new FakeRepository<VendorForm>();
            fakeRepository.Add(vendorFirstSave);

            var vendorUpdated = new VendorForm { Id = 1, Code = "ven update", Name = "vendor1 update" };
            var target = new FakeRepository<VendorForm>();

            // Act
            target.Add(vendorUpdated);

            // Assert
            List<dynamic> outlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out outlist);
            Assert.IsTrue(outlist.Count == 1);
            Assert.IsTrue(outlist[0].Id == 1);
            Assert.IsTrue(outlist[0].Code == "ven update");
            Assert.IsTrue(outlist[0].Name == "vendor1 update");
        }

        /// <summary>
        /// Save(T obj) FakeRepository &lt;T,X&gt; ---&gt;, X is single object --Retrieve Nested Item --Update
        ///   Test Save for FakeRepository &lt;T,X &gt; this test method test the method like when we update data for the productType in GUI we just select the vendor if we want to update it from drop down list that provide this product, we change the other data of the product and save it, 
        ///   remember that the vendor and the product already exist in our system and we only update product type to link to other vendor or change the product type data
        /// </summary>
        [TestMethod]
        public void SaveUpdateSingleComplextObjectHasOneObjectsThatRetriveItsDataByIdFromMemoryDbAndDiscardItsCurrentDataTest()
        {
            // Arrange
            var vendorPreTest = new VendorForm { Code = "ven 1", Name = "vendor 1", Address = "Address 1", Email = "Email 1" };
            var vendorFakeRepository = new FakeRepository<VendorForm>();
            vendorFakeRepository.Add(vendorPreTest);

            var productTypeFormPreSave = new ProductTypeForm { Name = "ProductTypeForm 1", VendorForm = vendorPreTest };
            var productTypeFakeRepository = new FakeRepository<ProductTypeForm, VendorForm>();
            productTypeFakeRepository.Add(productTypeFormPreSave);
            
            var vendor = new VendorForm { Id = 1, Name = "vendor1" };
            var productTypeForm = new ProductTypeForm { Id = 1, Name = "ProductTypeForm 1", VendorForm = vendor };
            var target = new FakeRepository<ProductTypeForm, VendorForm>();

            // Act
            target.Add(productTypeForm);

            // Assert
            List<dynamic> productTypeoutlist;
            List<dynamic> vendorOutlist;
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.ProductTypeForm", out productTypeoutlist);
            MemoryStorage.MemoryDb.TryGetValue("M.Radwan.EntitiesTest.VendorForm", out vendorOutlist);
            Assert.IsTrue(MemoryStorage.MemoryDb.Count == 2);
            Assert.IsTrue(productTypeoutlist.Count == 1);
            Assert.IsTrue(vendorOutlist.Count == 1);
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Email == "Email 1");
            Assert.IsTrue(productTypeoutlist[0].VendorForm.Email == vendorOutlist[0].Email);
            Assert.IsTrue(productTypeoutlist[0].Id == 1);
        }

        #endregion
    }
}
