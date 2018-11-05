# Loose coupling

Loose coupling is used for easier simultaneous development (more people working on same project) and easier and more reliant testing.

The idea is to make classes/modules/pieces of software/w-e you want to call it (i.e.: Game logic services, controllers, repositories, ...) less reliant on one another, make them more independent, easier to test and develop without depending on other classes.

Motivation

For example:

´
class GameController {
    public GameService _gameService;

    public GameController() {
        _gameService = new GameService(parameters);
    }

    public int SomeMethod() {
        // do something
        return _gameService.doSomethingForMe(withMyStuff);
    }
}
´

... class GameController now depends on GameService.

Imagine now GameController's method SomeMethod starts producing unexpected/wrong results. Our test for this method will fail, what we do is blame the guy, who last changed the method. But the bug might be inside GameService's method "doSomethingForMe". You can't tell from the tests or behaviour of SomeMethod.

Or imagine working on GameController's method SomeService, while your coworker is working on GameService. He is behind on schedule because of sick leave/poor time management etc. You can't finish your work, until he implements his method doSomethingForMe.

Now imagine we are using some class from some other team on our company, and they come up with an idea to re-write their GameService class and make a SuperGameService class. Awesome. But now, everywhere, where we use GameService, we will need to change it to SuperGameService. How about we could just have a "switch", where we could decide, which GameService to use - Super or old one? Pull the trigger and we have SuperGameService everywhere. DI will enable us that.

We see that it would be nice, if our classes (and thus ourselves), wouldn't depend on each other / wouldn't be so tightly connected / coupled.

Implementation

We will make our classes less dependant on one another by using Interfaces, instead of other classes as members of a certain class. Additionally, classes won't construct their members themselves - the responsibility is passed to someone else - control is inverted (read IoC).

In this case, the class we depend on will be injected through a constructor. There are other was to inject dependency, we decided to go for this kind, since it's the most simple and fits our needs perfectly fine.

so instead of example above, we will have:

´
class GameController {
    public IGameService _gameService;

    public GameController(IGameService _injectedGameServiceImplementation) {
        _gameService = _injectedGameServiceImplementation;
    }

    public int SomeMethod() {
        // do something
        return _gameService.doSomethingForMe(withMyStuff);
    }
}
´

We see that now any class that implements IGameService can now be passed inside a constructor of GameController. What good does the bring?

* We can switch between implementations really easily (SuperGameService example)

* We can test our class methods without worrying about bugs that might be in classes our class depends on and we can develop classes before dependencies are developed.

Example: We want to test the logic of method SomeMethod. We don't care if IGameService's method doSomethingForMe is working properly - the method might not have even been developed!
To only test SomeMethod's logic, we write a simple mock of a GameService. A class (lets name it MockGameService) that implements IGameService, but it's methods are mocked - they return static values, they don't really do anything, except provide us with expected results.
Now we know what to expect from mocked game service, and can write our tests for SomeMethod, independedly from the implementations of IGameService we might use in the future. We can even fully develop and test this method, before the IGameService's real implementation is developed.

... but who will pass the right implementation in constructors, depending on the context (testing --- deployed software)?

... Ninject dependency injection framework!

How?

Added as NuGet package, Ninject generates a file Ninject.Web.Common.cs inside App_Start folder for each Project it is added to. In this file, proper implementations can be bound to corresponding interfaces.

That way, you will bind the real GameService implementation to IGameService in the project that is "deployed, used" ...

´
kernel.Bind<IBoardService>().To<BoardService>();
´

... and mock implementation MockGameService to IGameService in test project

´
kernel.Bind<IBoardService>().To<MockBoardService>();
´
