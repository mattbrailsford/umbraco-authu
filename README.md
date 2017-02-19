# Umbraco.OAuth
Umbraco.OAuth is an add-on library to [Umbraco](https://github.com/umbraco/umbraco-cms) providing a simplified OAuth2 endpoint from which you can authenticate Umbraco Members/Users and use it to protect your API controllers, ideal for developing web/mobile apps.

## Installation

### Nuget

    PM> Install-Package Umbraco.OAuth

There's also a nightly Nuget feed: [`https://www.myget.org/F/umbraco-oauth/api/v2`](https://www.myget.org/F/umbraco-oauth/api/v2)

## Configuration
Umbraco.OAuth is configured using the `OAuth.ConfigureEndpoint` helper inside an Umbraco `ApplicationEventHandler` like so:
````csharp 
    public class Boostrap : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase app, ApplicationContext ctx)
        {
            OAuth.ConfigureEndpoint(...);
        }
    }
````
### Basic Configuration
For the most basic OAuth implementation, the following minimal configuration is all that is needed:
````csharp 
    OAuth.ConfigureEndpoint("/oauth/token", new OAuthOptions {
        UserService = new UmbracoMembersOAuthUserService(),
        SymmetricKey = "856FECBA3B06519C8DDDBC80BB080553",
        AccessTokenLifeTime = 20 // Minutes
    });
````

This will create an endpoint at the path `/oauth/token`, authenticating requests against the Umbraco members store, issuing access tokens with a lifespan of 20 minutes.

### Advanced Configuration
For a more advanced OAuth implementation, the following conifguration shows all the supported options.
````csharp 
    OAuth.ConfigureEndpoint("realm", "/oauth/token", new OAuthOptions {
        UserService = new UmbracoMembersOAuthUserService(),
        SymmetricKey = "856FECBA3B06519C8DDDBC80BB080553",
        AccessTokenLifeTime = 20, // Minutes
        ClientStore = new UmbracoDbOAuthClientStore(),
        RefreshTokenStore = new UmbracoDbOAuthRefreshTokenStore(),
        RefreshTokenLifeTime = 1440, // Minutes (1 day)
        AllowedOrigin = "*"
    });
````
This will create an endpoint the same as the basic configuration with added support of refresh tokens and a client store.

### Configuration Options
* __Realm : string__   
  _[optional, default:"default"]_  
  A uniqie alias for the configuration, allowing you to configure multiple endpoints.
* __Path : string__  
  _[optional, default:"/oauth/token"]_  
  The path of the endpoint (__IMPORTANT!__ Be sure to add this to the `umbracoReservedPaths` app setting)
* __UserService : IOAuthUserService__  
  _[optional, default:"new UmbracoMembersOAuthUserService()"]_  
  The service from which to validate authentication requests against. Out of the box Umbraco.OAuth comes with 2 implementations, `UmbracoMembersOAuthUserService` and `UmbracoUsersOAuthUserService` with authenticate against the Umbraco members and users store respectively. Custom sources can be configured by implementing the `IOAuthUserService` interface yourself.
* __SymmetricKey : string__  
  _[required]_  
  A symetric key used to sign the generated access tokens. Must be a string 32 characters long, BASE64 encoded.
* __AccessTokenLifeTime : int__  
  _[optional, default:20]_  
  Sets the lifespan, in minutes, of an access token before re-authentication is required. Should be short lived.
* __ClientStore : IOAuthClientStore__  
  _[optional, default:null]_  
  Defines a store for OAuth client information. If not null, all authentication requests are required to pass valid client credentials in order for authentication to be successful. If null, client crendetials are not required to authenticate. Out of the box Umbraco.OAuth comes with 2 implementations, `InMemoryOAuthClientStore` which stores a fixed list of client credentials in memory and `UmbracoDbOAuthClientStore` which stores the client credentials in a custom database table in the Umbraco database. Alternative implementations can be configured by implementing the `IOAuthClientStore` interface.
* __RefreshTokenStore : IOAuthRefreshTokenStore__  
  _[optional, default:null]_  
  Defines a store for OAuth refresh token information. If not null, authentication responses will include a `refresh_token` parameter which can be used to re-authenticate a user without needing to use their username / password credentials, allowing you to extend the lifespan of an access token. If null, refresh tokens will not be issued. Out of the box, Umbraco.OAuth comes with 1 implementation, `UmbracoDbOAuthRefreshTokenStore`, which stores the refresh tokens in a custom database table in the Umbraco database. Alternative implementations can be configured by implementing the `IOAuthRefreshTokenStore` interface.
* __RefreshTokenLifeTime : int__  
  _[optional, default:1440]_  
 Sets the lifespan, in minutes, of a refresh token before it can no longer be used. Can be long lived. If a client store is configured, this will get overridden by the client settings.
* __AllowedOrigin : string__  
  _[optional, default:"*"]_  
  Sets the allowed domain from which authentication requests can be made. If developing a web application, it is strongly recommended to set this to the domain from which your app it hosted at to prevent access from unwanted sources. If developing an app, can be set to wildcard "*" which will allow any source to access it, however it is strongly recommended you used a client store which requires a secret key to be passed. If a client store is configured, this will get overridden by the client settings.

## Usage
### Authenticating
With an endpoint configured, initial authentication can be performed by sending a POST request to the endpoints url with a body containing the following key values:
* __grant_type__ = "password"
* __username__ = The users username
* __password__ = The members password
* __client_id__ = A valid client id (Only required if a client store is configured)
* __client_secret__ = A valid client secret (Only required if a client store is configured, and the client is "secure")

Example (with client store and refresh token stores configured):

    Request URL:
    POST https://mydomain.com/oauth/token
    Request POST Body:
    grant_type=password&username=joebloggs&password=password1234&client_id=myclient&client_secret=myclientsecret
    Response:
    {
      "access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiIxMDgxIiwidW5pcXVlX25hbWUiOiJtZW1iZXIiLCJyb2xlIjoiTWVtYmVyIiwicmVhbG0iOiJkZWZhdWx0IiwiZXhwIjoxNDg3NDk2NzM3LCJuYmYiOjE0ODc0OTU1Mzd9.9uiIxrPggvH5nyLbH4UKIL52V6l5mpOyJ26J12FkXvI",
      "token_type": "bearer",
      "expires_in": 1200,
      "refresh_token": "b3cc9c66b86340c5b743f2a7cec9d2f1"
    }
    
A subsequent refresh token authentication request can be performed by sending a POST request to the endpoints url with a body containing the following key values:
* __grant_type__ = "refresh_token"
* __refresh_token__ = The refresh token returned from the original authentication request
* __client_id__ = A valid client id (Only required if a client store is configured)
* __client_secret__ = A valid client secret (Only required if a client store is configured, and the client is "secure")

Example (with client store and refresh token stores configured):

    Request URL:
    POST https://mydomain.com/oauth/token
    Request POST Body:
    grant_type=refresh_token&refresh_token=b3cc9c66b86340c5b743f2a7cec9d2f1&client_id=myclient&client_secret=myclientsecret
    Response:
    {
      "access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiIxMDgxIiwidW5pcXVlX25hbWUiOiJtZW1iZXIiLCJyb2xlIjoiTWVtYmVyIiwicmVhbG0iOiJkZWZhdWx0IiwiZXhwIjoxNDg3NDk2NzM3LCJuYmYiOjE0ODc0OTU1Mzd9.9uiIxrPggvH5nyLbH4UKIL52V6l5mpOyJ26J12FkXvI",
      "token_type": "bearer",
      "expires_in": 1200,
      "refresh_token": "b3cc9c66b86340c5b743f2a7cec9d2f1"
    }

If any authentication request fails, a status of `401 Unauthorized` will be returned containing error information in a JSON object.

### Protecting Your API Controllers
With your endpoint working and issuing tokens, you can protect your API controllers by adding the `OAuth` attribute to your classes, and then use the standard `Authorize` attribute to restrict action access as follows:
````csharp 
    [OAuth]
    public class MyApiController : UmbracoApiController
    {
        [HttpGet]
        [Authorize]
        public string HeloWorld()
        {
            return "Hello " + Members.GetCurrentMember()?.Name;
        }
    }
````
The `OAuth` attribute has a single optional parameter, `Realm`, which allows you to define which realm this controller is associated with. If a realm is configured, only access tokens that were generated from the associated endpoint will be valid.

### Accessing Protected Actions
To access a protected action, an `Authorization` header should be added to the request with a value of `Bearer {access_token}` were `{access_token}` is the value of the access token requested from the authentication endpoint.

Example:

    Request URL:
    POST https://mydomain.com/umbraco/api/myapi/helloworld
    Request Headers:
      Authorization: Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiIxMDgxIiwidW5pcXVlX25hbWUiOiJtZW1iZXIiLCJyb2xlIjoiTWVtYmVyIiwicmVhbG0iOiJkZWZhdWx0IiwiZXhwIjoxNDg3NDk2NzM3LCJuYmYiOjE0ODc0OTU1Mzd9.9uiIxrPggvH5nyLbH4UKIL52V6l5mpOyJ26J12FkXvI
    Response:
    "Hello Joe"

## Contributing To This Project

Anyone and everyone is welcome to contribute. Please take a moment to review the [guidelines for contributing](CONTRIBUTING.md).

* [Bug reports](CONTRIBUTING.md#bugs)
* [Feature requests](CONTRIBUTING.md#features)
* [Pull requests](CONTRIBUTING.md#pull-requests)

## License

Copyright &copy; 2017 Matt Brialsford, Outfield Digital Ltd 

Licensed under the [MIT License](LICENSE.md)