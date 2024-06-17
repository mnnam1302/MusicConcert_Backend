# Music Concert
## About
The main objective of this project is to represent the state-of-the-art of a **distributed**, **reliable**, and **highly scalable** system.

**Scalability** and **Resilience** require **low coupling** and **high cohesion**, principles strongly linked to the proper understanding of the business through **well-defined boundaries**, combined with a healthy and
efficient integration strategy such as **Event-driven Architecture** (EDA).

**Independence**, as the main characteristic of a **Microservice**, can only be found in a **Bounded Context**.

The [**Event Sourcing**](https://www.eventstore.com/event-sourcing) is a proprietary implementation that, in addition to being **naturally auditable** and **data-driven**, represents the most efficient persistence mechanism ever. An **eventual state
transition** Aggregate design is essential at this point. The **Event Store** comprises EF Core (ORM) + MSSQL (Database).

As a domain-centric approach, Clean Architecture provides the appropriate isolation between the Core (Application + Domain) and "times many" Infrastructure concerns.

### Give a Star! 

Support this research by **giving it a star**. Thanks!

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

### Domain Driven Design (DDD)
> Domain-Driven Design is an approach to software development that centers the development on programming a domain model that has a rich understanding of the processes and rules of a domain. The name
> comes from a 2003 book by Eric Evans that describes the approach through a catalog of patterns. Since then a community of practitioners have further developed the ideas, spawning various other books
> and training courses. The approach is particularly suited to complex domains, where a lot of often-messy logic needs to be organized.
>
> [Fowler, Martin. "DomainDrivenDesign", _martinfowler.com_, last edited on 22 April 2020](https://martinfowler.com/bliki/DomainDrivenDesign.html)

#### Bounded Context

> Basically, the idea behind bounded context is to put a clear delineation between one model and another model. This delineation and boundary that's put around a domain model, makes the model that is
> inside the boundary very explicit with very clear meaning as to the concepts, the elements of the model, and the way that the team, including domain experts, think about the model.
>
> You'll find a ubiquitous language that is spoken by the team and that is modeled in software by the team. In scenarios and discussions where somebody says, for example, "product," they know in that
> context exactly what product means. In another context, product can have a different meaning, one that was defined by another team. The product may share identities across bounded contexts, but,
> generally speaking, the product in another context has at least a slightly different meaning, and possibly even a vastly different meaning.
>
> [Vernon, Vaughn. "Modeling Uncertainty with Reactive DDD", _www.infoq.com_, last edited on 29 Set 2018](https://martinfowler.com/bliki/BoundedContext.html)

![](https://raw.githubusercontent.com/AntonioFalcaoJr/EventualShop/release/.assets/img/BoundedContext.jpg)  
[Fig. 5: Martin, Fowler. _BoundedContext_](https://martinfowler.com/bliki/BoundedContext.html)

#### Aggregate

> I think a model is a set of related concepts that can be applied to solve a problem.
> -- <cite> Eric Evans </cite>

![](https://raw.githubusercontent.com/AntonioFalcaoJr/EventualShop/release/.assets/img/aggregate.png)  
Fig. 6: Vernon, V. (2016), Aggregates from Domain-Driven Design Distilled, 1st ed, p78.

> Each Aggregate forms a transactional consistency boundary. This means that within a single Aggregate, all composed parts must be consistent, according to business rules, when the controlling
> transaction is committed to the database. This doesn't necessarily mean that you are not supposed to compose other elements within an Aggregate that don't need to be consistent after a transaction.
> After all, an Aggregate also models a conceptual whole. But you should be first and foremost concerned with transactional consistency. The outer boundary drawn around Aggregate Type 1 and Aggregate
> Type 2 represents a separate transaction that will be in control of atomically persisting each object cluster.
>
> Vernon, V. (2016) Domain-Driven Design Distilled, 1st ed. New York: Addison-Wesley Professional, p78.

> Aggregate is a pattern in Domain-Driven Design. A DDD aggregate is a cluster of domain objects that can be treated as a single unit. An example may be an order and its line-items, these will be
> separate objects, but it's useful to treat the order (together with its line items) as a single aggregate.
>
> [Fowler, Martin. "DDD_Aggregate", _martinfowler.com_, last edited on 08 Jun 2015](https://martinfowler.com/bliki/DomainDrivenDesign.html)

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
