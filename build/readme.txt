===============================================================
 Umbraco.OAuth
===============================================================

Umbraco.OAuth has been installed!

Before you begin, you'll need to make an app setting config change, adding the ~/oauth/ path to the umbracoReservedPaths setting.

Next, you'll need to configure the OAuth service inside an Umbraco application event handler like so:

public class Boostrap : ApplicationEventHandler
{
    protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
    {
        OAuthConfig.Configure(new OAuthConfigOptions
        {
            UserService = new UmbracoMembersOAuthUserService(),
            SymmetricKey = "856FECBA3B06519C8DDDBC80BB080553",
            AccessTokenLifeTime = 20, // Minutes
            RefreshTokenStore = new UmbracoDbOAuthRefreshTokenStore(),
            RefreshTokenLifeTime = 1440, // Minutes (1 day)
            TokenEndpoint = "/oauth/token",
            AllowedOrigin = "*"
        });
    }
}

Out of all the config options, the only one that is required is the SymmetricKey. This MUST be a string of 32 characters length, BASE64 encoded. The rest of the options shown represent their defailt values, except RefreshTokenStore, which defaults to null (meaning don't use refresh tokens). The UserService paramater declares an IOAuthUserService instance used to validate user logins against. Out of the box there are 2 services defined, UmbracoMembersOAuthUserService and UmbracoUsersOAuthUserService, however you can define your own if you wish to use a custom user store. The RefreshTokenStore declares an IOAuthRefreshTokenStore instanced used to store refresh tokens. Out of the box there is one store defined, UmbracoDbOAuthRefreshTokenStore, which stores the tokens in the Umbraco database via a PetaPoco model. If you need to store the tokens in an alternative location you can implement the interface yourself and pass your instance into the config. If the RefreshTokenStore parameter is null, this will disable the refresh token feature and only standard username / password logins will be supported. The AccessTokenLifeTime and RefreshTokenLifeTime parameters declare the length in minutes that access token and refresh tokens should be valid for. Access tokens should be short lived, where as refresh tokens can last several days. TokenEndpoint defines the url of the oauth token end point. If you change this then be sure to update the umbracoReservedPaths app setting as well. AllowedOrigin defines the domain from which requests are allowed to come from. If you are building an app, you can set it to "*" to allow all origins, but if you are building a web based application, it is advised to set this to the domain of your app to prevent unpermitted access.

With OAuth configured, you are now set to request OAuth token from the end point. To request a token, a post request should be made to /oauth2/token with a body containing the following key values:

* grant_type = password
* username = member user name
* password = member password

If a token expires, and a refresh token store is configured, you can request a refresh token by posting to /oauth2/token with a body containing the following key values:

* grant_type = refresh_token
* refresh_token = the refresh token from the original token auth request

Now that you can authenticate you'll want to add protection to your controllers by adding the [OAuth] attribute to your class declaration and then using the standard [Authorize] attribute to protect your methods. To call a protected method, you'll need to add the following header to your requests:

* Authorization = "Bearer " + the access_token returned from the last auth token request