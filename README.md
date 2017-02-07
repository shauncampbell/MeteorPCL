# MeteorPCL
A PCL library for communicating with Meteor Servers 

This library started life as a fork of [OzTKs DDPClient.NET](https://github.com/OzTK/DDPClient.NET) that I made whilst working on my [RocketChatPCL bindings for Xamarin](https://github.com/shauncampbell/RocketChatPCL) project. Unfortunately, to get it working with my project I had to effectively reorganise the source code - so its not a proper fork, but I wanted to make the changes available to everyone. The project has the following major differences:

* Removal of the dynamic keyword as that was causing issues when creating a PCL library.
* Added the ability to wait on the result of a call.
* Added in checks to ensure that the Websocket is alive.
* Added in a Meteor friendly wrapper.

## Dependencies
This project has the following dependencies:

* [Newtonsoft.Json version 9.0.0+](https://github.com/JamesNK/Newtonsoft.Json)
* [Websocket4NET version 0.14.1](https://github.com/kerryjiang/WebSocket4Net)

## Usage
```c#
var meteor = new Meteor();
meteor.Connect("ddp.test.com", true, "TestLoginToken")
      .ContinueWith((login) => {
        Debug.WriteLine("Logged in!");
      });
```

## License

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
