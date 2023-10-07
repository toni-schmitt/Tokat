Sadly I got sick and could not finish my implementation fot the WeatherApi-Service. It is almost done, the last thing
you have to do is to adapt the get-endpoint to dynamically use one of the services.
If one service is down we want to request weather from another service and try that until we get a response,
or we return NotFound if we are trough each service.
