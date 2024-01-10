# ATENTION: This repo was used for different tasks

# Task 7: APM tools - Azure Application Insights
Clone or checkout the last commit, configure an Azure account and create an Application Insights app, go to the project appsettings.json file, provide ConnectionString and InstrumentationKey inside the ApplicationInsights object and run the API.

Hitting the endpoints will generate traces, logs and custom events logs depending on the API version and method.

### Task 6

New endpoint for achieving API gateway module requirements.

### Carting / Task 4 (Message broker Listener)

Follow the steps in the [Catalog service project](https://github.com/camilopv19/Task2-CatalogService) and run it.
Review and checkout last commit. 

#

On the Carting folder, run the project by executing the CLI command `dotnet run`.

For this version of the project, a swagger page (for previous Cart API) and a default console will be displayed.

If one message is received (from an updated item in the Catalog service/project), the respective operations will be executed and the results will be logged on the console.

In the swagger page (or using any http/REST client) the given item will be inserted or updated according the existence of the Cart Id. So it's recommended to run the Get Carts method to see the changes.

### Carting / Task 1

Review and checkout first commits.
