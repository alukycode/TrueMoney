nice article explaining how to unit-test classes that use EntityFramework: http://www.nogginbox.co.uk/blog/mocking-entity-framework-data-context
extension methods for IQueryable: http://www.nogginbox.co.uk/blog/extension-methods-filter-iqueryable


on StackOweflow people say that it isn't possible to mock EF, but i didn't understand what the fuck
http://stackoverflow.com/questions/15791303/how-to-unit-test-a-repository-pattern-that-uses-entity-framework
http://stackoverflow.com/questions/13332002/how-to-mock-the-limitations-of-entityframeworks-implementation-of-iqueryable/13352779#13352779
seems like we can't really test Include method http://stackoverflow.com/questions/37026222/mocking-entity-framework-repository-pattern


library to use In-memory database instead of mocking EntityFramework DbSet


EF already implements Repository pattern: http://programmers.stackexchange.com/questions/180851/why-shouldnt-i-use-the-repository-pattern-with-entity-framework/220126


When using a database, use NHibernate’s ISession directly
Encapsulate complex queries into query objects that construct an ICriteria query that I can get and manipulate further
When using something other than a database, create a DAO for that, respecting the underlying storage implementation
Don’t try to protect developers
(c) https://ayende.com/blog/3955/repository-is-the-new-singleton
https://web.archive.org/web/20100103212029/http://davybrion.com/blog/2009/04/educate-developers-instead-of-protecting-them/


useful comment from Reddit:
I'm in the "Repository is dead" camp.
This is what I'd recommend:
- Inject a DbContext into your service classes.
- 1 DbContext per HTTP request. Some DI container config is required here.
- Make sure DbContext.SaveChanges() is automatically called at the end of the request and don't call it anywhere else.
- For testability of your service classes, you can create an interface for your DbContext to implement but you don't have to as DbContext is created to be mockable. You can mock virtual methods and the DbContext methods are virtual. see http://msdn.microsoft.com/en-au/data/dn314429.aspx
You don't get the ability to easily switch ORMs but I'd argue that it's not worth the extra cost of supporting multiple ORMs.
https://www.reddit.com/r/dotnet/comments/2sk03a/entity_framework_dependency_injection_without/