# Music Concert
## About
The main objective of this project is to represent the state-of-the-art of a **distributed**, **reliable**, and **highly scalable** system.

**Scalability** and **Resilience** require **low coupling** and **high cohesion**, principles strongly linked to the proper understanding of the business through **well-defined boundaries**, combined with a healthy and
efficient integration strategy such as **Event-driven Architecture** (EDA).

**Independence**, as the main characteristic of a **Microservice**, can only be found in a **Bounded Context**.

The [**Event Sourcing**](https://www.eventstore.com/event-sourcing) is a proprietary implementation that, in addition to being **naturally auditable** and **data-driven**, represents the most efficient persistence mechanism ever. An **eventual state
transition** Aggregate design is essential at this point. The **Event Store** comprises EF Core (ORM) + MSSQL (Database).

As a domain-centric approach, Clean Architecture provides the appropriate isolation between the Core (Application + Domain) and "times many" Infrastructure concerns.


## The Solution Architecture
### V1
![](https://github.com/mnnam1302/MusicConcert_Backend/blob/dev/.assets/img/solution_architecture_v1.png)

### V1 Details
![](https://github.com/mnnam1302/MusicConcert_Backend/blob/dev/.assets/img/solution_architecture_v1_details.png)

## Installation
> Run docker-compose to build Infrastructure such as Redis, MSSQL Server, RabbitMQ, Seq for development environment.
```
docker-compose -f docker-compose.Development.Infrastructure.yaml up -d
```

## Research

### Domain Driven Design

### Event Sourcing
> Instead of storing just the current state of the data in a domain, use an append-only store to record the full series of actions taken on that data. The store acts as the system of record and can be
> used to materialize the domain objects. This can simplify tasks in complex domains, by avoiding the need to synchronize the data model and the business domain, while improving performance,
> scalability, and responsiveness. It can also provide consistency for transactional data, and maintain full audit trails and history that can enable compensating actions.
>
> ["Event Sourcing pattern" _MSDN_, Microsoft Docs, last edited on 23 Jun 2017](https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing)

> Event Sourcing ensures that all changes to application state are stored as a sequence of events. Not just can we query these events, we can also use the event log to reconstruct past states, and as
> a foundation to automatically adjust the state to cope with retroactive changes.
>
> [Fowler, Martin. "Eventsourcing", _martinfowler.com_, last edited on 12 Dec 2005](https://martinfowler.com/eaaDev/EventSourcing.html)

![](https://raw.githubusercontent.com/AntonioFalcaoJr/EventualShop/release/.assets/img/event-sourcing-overview.png)      
[Fig. 7: MSDN. _Event Sourcing pattern_](https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing#solution)

### Event Driven Architecture
> Event-driven architecture (EDA) is a software architecture paradigm promoting the production, detection, consumption of, and reaction to events. An event can be defined as "a significant change in
> state".
>
> ["Event-driven architecture." _Wikipedia_, Wikimedia Foundation, last edited on 9 May 2021](https://en.wikipedia.org/wiki/Event-driven_architecture)

> Event-driven architecture refers to a system of loosely coupled microservices that exchange information between each other through the production and consumption of events. An event-driven system
> enables messages to be ingested into the event driven ecosystem and then broadcast out to whichever services are interested in receiving them.
>
> [Jansen, Grace & Saladas, Johanna. "Advantages of the event-driven architecture pattern." _developer.ibm.com_, IBM Developer, last edited on 12 May 2021](https://developer.ibm.com/articles/advantages-of-an-event-driven-architecture)

![](https://raw.githubusercontent.com/AntonioFalcaoJr/EventualShop/release/.assets/img/eda.png)  
[Fig. 15: Uit de Bos, Oskar. _A simple illustration of events using the publish/subscribe messagingmodel_](https://medium.com/swlh/the-engineers-guide-to-event-driven-architectures-benefits-and-challenges-3e96ded8568b)

### EDA & Event Sourcing

> Event sourcing a system means the treatment of events as the source of truth. In principle, until an event is made durable within the system, it cannot be processed any further. Just like an
> author’s story is not a story at all until it’s written, an event should not be projected, replayed, published or otherwise processed until it’s durable enough such as being persisted to a data
> store. Other designs where the event is secondary cannot rightfully claim to be event sourced but instead merely an event-logging system.
>
> Combining EDA with the event-sourcing pattern is another increment of the system’s design because of the alignment of the EDA principle that events are the units of change and the event-sourcing
> principle that events should be stored first and foremost.
>
> [Go, Jayson. "From Monolith to Event-Driven: Finding Seams in Your Future Architecture", _InfoQ_, last edited on 15 Set 2020](https://www.eventstore.com/blog/what-is-event-sourcing)

Comparison overview:

| Aspects | Event sourcing            | EDA                             |
|---------|---------------------------|---------------------------------|
| Propose | Keeping history           | Highly adaptable and scalable   |
| Scope   | Single application/system | Whole organisation/several apps |
| Storage | Central event store       | Decentralised                   |
| Testing | Easier                    | Harder                          |

### Clean Architecture

> Clean architecture is a software design philosophy that separates the elements of a design into ring levels. An important goal of clean architecture is to provide developers with a way to organize
> code in such a way that it encapsulates the business logic but keeps it separate from the delivery mechanism.
>
> The main rule of clean architecture is that code dependencies can only move from the outer levels inward. Code on the inner layers can have no knowledge of functions on the outer layers. The
> variables, functions and classes (any entities) that exist in the outer layers can not be mentioned in the more inward levels. It is recommended that data formats also stay separate between levels.
>
> ["Clean Architecture." _Whatis_, last edited on 10 Mar 2019](https://whatis.techtarget.com/definition/clean-architecture)

![](https://github.com/mnnam1302/MusicConcert_Backend/blob/dev/.assets/img/CleanArchitecture.jpg)  
[Fig. 28: C. Martin, Robert. _The Clean Architecture_](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## :toolbox: Tech Stack

- [ASP.NET Core 7](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-net-7-preview-1/) - A free, cross-platform and open-source web-development framework;
- [EF Core 7](https://devblogs.microsoft.com/dotnet/announcing-ef7/) - An open source object–relational mapping framework for ADO.NET;
- [MSSQL](https://hub.docker.com/_/microsoft-mssql-server) - A relational database management system (Event Store Database);
- [MassTransit](https://masstransit-project.com/) - Message Bus;
- [FluentValidation](https://fluentvalidation.net/) - A popular .NET library for building strongly-typed validation rules;
- [Serilog](https://serilog.net/) - A diagnostic logging to files, console and elsewhere.