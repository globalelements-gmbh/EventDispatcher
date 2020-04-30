# Event Dispatcher

[![Build Status](https://api.travis-ci.org/globalelements-gmbh/EventDispatcher.svg?branch=master "Build status")](https://travis-ci.org/github/globalelements-gmbh/EventDispatcher)

## What is this package?

This package is an event dispatcher. It allows to subscribe to events using StructureMap and dependency injection/assembly scanning.

```c#
// instantiate the event dispatcher and scan the assembly
var eventDispatcher = new EventDispatcher(new StructureMap.Container());
eventDispatcher.Scan();

// some event class
class Event : IEvent {}

// dispatch the event
var eventObj = eventDispatcher.Dispatch(new Event)
```

## License

Copyright 2020 Global Elements GmbH

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
