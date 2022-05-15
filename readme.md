#WGU Mobile Portal
Mockup of a college scheduling app made with Xamarin.Forms. This was course work for WGU.

# WGU Mobile Portal

This project is a mockup of a college course scheduling app, made with C# and Xamarin.Forms. This was course work for WGU. This project has not been published to the Google Play store, as it currently includes resources copyrighted by WGU.

Currently, only a Android project is available for this, as that was the only requirement for the course; I also do not have a readily availble platform for testing the functionality against iphones.

To run and compile this, you will need Visual Studio. I have tested against Visual Studio 2019 and Visual Studio 2022, I do not know if this will compile against older versions in it's current state.

## Design

As this was designed to follow a relatively strict rubric, this project has some... unconventional design decisions. Given the strictness of the rubric, fitting this into an MVVM pattern was much harder than it needed to be, as no third party MVVM frameworks were allowed. Even so, atleast in the Xamarin.Forms world, consenses shows MVVM as the best practice for this framework - and if I am going to do something (especially in an educational context!), it really only makes sense to do it the "right way".

## Future

Time permitting, I have some plans for this application.

1. **.NET MAUI conversion**. As Xamarin.Forms is being deprecated, I would like to convert this application to .NET MAUI.

2. **Generalize everything**. Right now, this has branding and other things specific to WGU. However, I have used lots of opensource resources for this and other projects. Turning this into a good reference project for an application dealing with CRUD operation, with best practices in use, could potetnailly help someone else in the future.

3. **Tests**. I did not write tests for this. I have several excuses, such as time contstriants, unfamiliarity with Xamarin.Forms, unfamiliarity with Android or Xamarin testing, etc.. Still, at the very least, some unit tests should be written, and incorporated into a pipeline.

4. **Publish**. Publish this (free) to the Google Play and the Apple App Store. Will probably do this as a CI/CD pipeline, commit -> testing -> release. This step depends on completing the first three.