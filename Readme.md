# SuperTracker Demo

This project is a test project to demonstrate a communication between 2 services.
The services are the **PixelService** and the **StorageService**.
The **PixelService** is responsible for collecting data from incoming request and sending it to the **StorageService**.
The **StorageService** is responsible for storing the data.

## Projects

Within this solution are the following projects:

1. **SuperTracker.PixelService**: PixelService built as Miminal API.

2. **SuperTracker.Domain**: A shared project storing events.

3. **SuperTracker.StorageService**: StorageService plays a receiver's role.

## Test Projects

1. **SuperTracker.PixesService.Integration.Tests**: Example how Specflow tests can be used.

2. **SuperTracker.PixelService.Tests**: A small project with unit tests of **PixelService**.

3. **SuperTracker.StorageService.Tests**: A small project with unit tests of **StorageService**.