# Guidelines for unit testing

The goal of the tests in our solution is mostly to ensure our that through our collaborative work, we don't unintendedly
break others' functionalities.

Thus, the tests are mostly aimed towards ensuring methods to return correct types and throw the right exceptions.
If the change was to be made that would break some functionality, such as change of return type -> we wouldn't be able to render
the response properly client-side because of that for example, there should be a test failing, signaling a developer that he had
done something wrong.

## How to write tests

* only test one functionality (a single method of a single class)
* if your class is dependant on another one, ensure to write a propr mockup of your dependency and inject it ninject startup class of the test project
* test both positive and negative scenarios

## Technical stuff

### Structure of a test method

Method should be divided into 3 logical parts: Arrange, Act and Assert, in this order.

```C#
[TestMethod]
public void MyTestMethod()
{
    // Arrange
    // We prepare an instance of our class and required data structures we need to pass in its method we wish to test
    // If necessary we also prepare expected results to compare them in the next step
    MyClass instanceOfMyClass = kernel.Get<MyClass>();
    DataClass parameters = new DataClass {
        data1 = 1,
        data2 = "feri"
    };

    // Act
    // We call the method and receive some response
    var response = instanceOfMyClass.MedthodIWishToTest(parameters);

    // Assert
    // We evaluate the correctness of the response (not null, right type, right data, correct exception thrown if negative scenario)
    Assert.IsNotNull(response);
}
```

### Full structure of a test class & method(s)

```C#
namespace FeriChess.Tests.Controllers
{
    [TestClass]
    public class MyClassTest
    {
        private IKernel kernel = new StandardKernel(new App_Start.NinjectWebCommon.TestModule());

        [TestMethod]
        public void MyTestMethod()
        {
            // method
        }
    }
}
```

### Getting an instance of your class from ninject

```C#
MyClass instanceOfMyClass = kernel.Get<MyClass>();
```

### Binding mock implementation to interface

...in `FeriChess.Tests/App_start/Ninject.Web.Common.cs`

```C#
/// <summary>
/// Load your modules or register your services here!
/// </summary>
/// <param name="kernel">The kernel.</param>
private static void RegisterServices(IKernel kernel)
{
    // add line here
    kernel.Bind<IInterfaceOfMyDependency>().To<MockImplementationOfMyDependency>().InSingletonScope();
}

public class TestModule : NinjectModule
{
    public override void Load()
    {
        // and here
        Bind<IInterfaceOfMyDependency>().To<MockImplementationOfMyDependency>().InSingletonScope();
    }
}
```