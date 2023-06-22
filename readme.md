# Identity Management

A repository to show how identity management using OIDC works when using:

* nextjs
* duende identity server <https://docs.duendesoftware.com/>
* aspnetcore

## Folder Structure

* web: contains the web frontend build with nextjs and react
* backend: contains the apis build with apsnetcore
* identity: contains the identity server build with aspnetcore and duende identity server

## Quick start

* Ensure running postgres on localhost:5432, user: postgres, password: docker
* Run identity server migrations
  * `cd identity/`
  * `dotnet tool restore`
  * `cd src/IdentityServer`
  * `dotnet ef database update`
* Start Identity Server
* Start Api1
* Start Api2
* Start Web frontend
