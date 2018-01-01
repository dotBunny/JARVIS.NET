# JARVIS.NET
Just A Rather Very Intelligent System

A rebuilt version of our JARVIS, utillizing .NET for cross-platform compatibility. Utilizing the full spectrum of the .NET Framework / .NET Standard / .NET Core, and platform specific libraries, JARVIS can be whatever you want it to be.
Follows the [.NET Coding Style](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md) guidelines.

## Projects
JARVIS is broken down into a variety of interconnected projects which make the codebase a little bit easier to maintain.

### CLI
Console based applications which run on a minimal footprint of .NET (.NET Core), allowing for ease of distribution.

#### JARVIS
The core of the JARVIS system. It allows for warehousing of different types of data, replaying of commands across different protocol, and anything else really.
  
#### JARVIS.Shard
A _shard_ is a simplified node which connects to JARVIS allowing for commands to be executed by JARVIS on the host machine. An example of this would be a remote server outside of a controlled network which needs to serve some form of data sync'd with JARVIS.

### Clients
Rich UI based applications which often are platform specific and dependant.

#### JARVIS.Client.Mac
A macOS client which encompasses the _shard_ functionality, with some added platform features. An example use case for this project, would be the manipulation of Wirecast scenes/layers/data.

### Libraries
Collections of common functionality used throughout JARVIS

#### JARVIS.Client
Shared client functionality across the different platforms.
  
#### JARVIS.Shared
A collection of shared functionality used across all projects.
  
#### JARVIS.Core
The core functionality of JARVIS is stored in a library for portability, and upgradability.