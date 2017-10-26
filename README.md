# JARVIS.NET
Just A Rather Very Intelligent System

A rebuilt version of JARVIS, utillizing .NET for cross-platform compatibility.

## JARVIS.Server ##
The central processing unit of the system, warehousing data, replaying commands, and anything else needed.

## JARVIS.Shard ##
A _shard_ is a node that connects to the server allowing for commands to be executed by the server on the node machine. An example use case for this setup, would be a dedicated stream processing box needing to be issued specific commands to adjust the scenes/layers/etc.

## JARVIS.Shared ##
A collection of shared functionality used throughout the JARVIS.

# System Requirements #

## Pi ##
```bash
$ sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
$ echo "deb http://download.mono-project.com/repo/debian jessie main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
$ sudo apt-get update
$ sudo apt-get install mono-runtime libmono-system-core4.0-cil libmono-system-runtime4.0-cil libmono-corlib4.0-cil
```
