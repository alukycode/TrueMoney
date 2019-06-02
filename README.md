# TrueMoney

Проект по предмету ТОФИ (преподаватель - Лещёв)

БГУИР, ФКСиС, ИиТП, 9 семестр

2016 г.

*ASP.NET MVC 5, Entity Framework 6, Castle Windsor, AutoMapper*

Планировалось, что получится нормальный проект с пиздатой архитектурой, но мы постарались и сделали кусок говна!

Что в нём хоть немного может быть полезным:
- bat file to configure iis
- custom error pages
- multi-layered architecture
- entity framework usage for repository layer (see readme.txt in TrueMoney.Data project)
- dependency injection using castle windsor for default asp.net template
- connection strings to localdb / sql server express
- photo upload
- simple implementation of global notifications
- FakeDbSet for unit-testing (to remove dependency on database)
