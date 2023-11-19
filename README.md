# ATENTION: This repo was used for different tasks

### Carting / Task 1

Review and checkout first commits.

### Carting / Task 4 (Message broker Listener)

Follow the steps in the [Catalog service project](https://github.com/camilopv19/Task2-CatalogService) and run it.
Review and checkout last commit. 

#

On the Carting folder, run the project by executing the CLI command `dotnet run`.

For this version of the project, a swagger page (for previous Cart API) and a default console will be displayed.

If one message is received (from an updated item in the Catalog service/project), the respective operations will be executed and the results will be logged on the console.

In the swagger page (or using any http/REST client) the given item will be inserted or updated according the existence of the Cart Id. So it's recommended to run the Get Carts method to see the changes.
