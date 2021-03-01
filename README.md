# Event Dispatcher

[![Build Status](https://teamcity-public.globalelements.ch/app/rest/builds/buildType:(id:EventDispatcher_BuildAndTest)/statusIcon.svg)](https://teamcity-public.globalelements.ch/buildConfiguration/EventDispatcher_BuildAndTest)
[![NuGet Version](https://img.shields.io/nuget/v/GlobalElements.EventDispatcher)](https://www.nuget.org/packages/GlobalElements.EventDispatcher)
[![NuGet Downloads](https://img.shields.io/nuget/dt/GlobalElements.EventDispatcher)](https://www.nuget.org/packages/GlobalElements.EventDispatcher)

## What is this package?

This package is an event dispatcher. It allows to subscribe to events using StructureMap and dependency injection/assembly scanning.

```c#
// instantiate the event dispatcher and scan the assembly
var eventDispatcher = new EventDispatcher(new Lamar.IContainer());
eventDispatcher.Scan(); // scanning is optional and happens on first call to .Dispatch()

// some event class
class Event : IEvent {}

// some event listener or subscriber
//  IEventListener      -> must be added manually to the dispatcher using .AddListener()
//  IEventSubscriber    -> is added automatically to the dispatcher when .Scan() is called
class EventListener : IEventListener { }
class EventSubscriber : IEventSubscriber { }

// dispatch the event
var eventObj = eventDispatcher.Dispatch(new Event())
```

## License

Copyright 2020-2021 Global Elements GmbH

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
