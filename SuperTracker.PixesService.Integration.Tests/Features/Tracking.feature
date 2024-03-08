Feature: Tracking
	Simple example of how specflow tests can be done for the PixelService project

Scenario: Track a request
	Given A client's application is going to get the tracking pixel
	When The client application requests the tracking pixel
	Then A transparent gif is returned