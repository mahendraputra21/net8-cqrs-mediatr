# DewaEShop Api
![Clean Architecture](https://github.com/mahendraputra21/net8-cqrs-mediatr/raw/clean-architecture/Images/Clean%20Architecture.png)

In this repository, I have implemented Clean Architecture principles, which prioritize the separation of business logic from implementation details such as frameworks and databases. This approach fosters modular, testable, and maintainable systems, ensuring flexibility and scalability as project requirements evolve.

# Features
- .NET 8
- CQRS (Command Query Responsibility Segregation)
- New Mediator (https://github.com/martinothamar/Mediator)
- Validation Behaviour
- Logging Behavior
- DBContext Transaction Pipeline Behavior
- Global Exception Handler
- Unit Of Work
- Sendgrid Integration
- Domain Driven Design (DDD)
- Standard API Response
- Auto Mapper
- Simple ASP.NET Core Identity

# Benefits of CQRS:
- Scalability: Allows scaling read and write operations independently based on their specific requirements.
- Performance: Optimizes read and write operations for their respective use cases, potentially improving application performance.
- Flexibility: Facilitates evolving and adapting the application as requirements change, by decoupling read and write logic.
- Complex Domain Logic: Supports handling complex domain logic by segregating concerns and focusing models on specific responsibilities.
