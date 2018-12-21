
# SampleApp

This sample application is designed to help anyone get an application up and running quickly with BOS. The SampleApp has a bare bones UI and focuses on stringing together BOS Auth API for Authentication and Authorization, the BOS Demographics API for profile management, and the BOS DAMS Api for an assest management style example. This is a ASP.Net Core 2.1 application organized with [Clean Architecture][clean-architecture]. All of the API Calls that are made to BOS require a BOS API Key. An appsettings.Development.json property for the key is defined, it just needs to have the key inserted. The application also uses SendGrid to send the welcome emails, and will need a SendGrid API key to work as well.

This application can be a guidline to implementing BOS or cloned and stipped of anything that you do not need and used as a starting point for building your application.

[clean-architecture]: https://github.com/ardalis/CleanArchitecture