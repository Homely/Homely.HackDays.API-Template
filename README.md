# Homely.API.Templates
Templates to be used to be build APIs, inspired by the awesome [ASP.NET Core Boilerplate Templates](https://github.com/ASP-NET-Core-Boilerplate/Templates). 
[<img src="https://homelyau.visualstudio.com/_apis/public/build/definitions/f23d4256-dd5b-497f-bba0-2c0098c5e3be/2/badge"/>](https://homelyau.visualstudio.com/API%20Template/_build/index?definitionId=2)


## Current configuration
### Logging
- [Serilog](https://serilog.net/) (outputting to [Seq](https://getseq.net/))

### Discoverability
- [Swagger](https://swagger.io/) (via [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle))

### Monitoring
- [ApplicationInsights](https://azure.microsoft.com/en-au/services/application-insights/)
- [Prefix](https://stackify.com/prefix/)

### Validation
- [FluentValidation](https://github.com/JeremySkinner/FluentValidation)

## Soon/considerations....
- [Health checks](https://github.com/dotnet-architecture/HealthChecks)
- Containers
- [Automapper](http://automapper.org/)
- [Resiliency](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/)
- [MediatR](https://github.com/jbogard/MediatR)
- Auth
