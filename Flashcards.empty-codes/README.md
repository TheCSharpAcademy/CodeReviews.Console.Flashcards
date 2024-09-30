

## Challenges faced + Lessons Learned
he error is likely occurring because you're using ConfigurationManager directly in a .NET Core or later project, which doesn't support it the same way as in .NET Framework applications.

In .NET Core and .NET 5/6/7/8, app.config is replaced by appsettings.json, and configuration is handled via the IConfiguration interface from the Microsoft.Extensions.Configuration namespace. ConfigurationManager is not typically used directly in modern .NET Core projects.