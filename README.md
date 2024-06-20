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
