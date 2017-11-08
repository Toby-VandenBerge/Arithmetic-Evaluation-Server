Arithmetic Evaluation
============
TCP server to evaluate arithmetic expressions

Behavior:
- The server accepts n number of connections (made from the Arithmetic.Evaluation.Client) on the configuration port (defaults to 1337)
- The server expects arithmetic expression requests from the clients
- The server evaluations the requested expressions and responds with the result
- The client can lose conection to the server and will continue to attempt to reconnect every 5 seconds
- Both the client and server can be gracefully stopped by a (Ctrl + c) or (Ctrl + break) command

Building
============
### Get .NET Core SDK with .NET 2.0+ runtime

####Windows
```
choco install dotnet-sdk -y
```

####Linux

Please see [Prerequisites for .NET Core on Linux](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x) for your target distro.

### Clone Repo
```
git clone https://github.com/Toby-VandenBerge/Arithmetic.Evaluation.git
```

### Build
Utilize [Cake](https://github.com/cake-build/cake) to execute the compilation and tests
#### Windows
```
cd arithmetic.evaluation
powershell build.ps1
```

#### Linux
This will require curl and mono to be installed
```
sudo apt-get install curl
```
```
sudo apt-get install mono-devel
```

```
cd arithmetic.evaluation
# Adjust the permissions
chmod +x build.sh

./build.sh
```

Running
============
There are two (2) applications:
### Server
Port defaults to 1337
Configuration can be found at 
```
src/Arithmetic.Evaluation.Server/appsettings.json
```

#### Start
##### Windows
```
run-server.bat
```
##### Linux
```
run-server.sh
```

### Client
It is possible to have n number of clients executing concurrently.

EvaluationServerIp defaults to '127.0.0.1'
EvaluationServerPort defaults to 1337
Configuration can be found at 
```
src/Arithmetic.Evaluation.Client/appsettings.json
```

#### Start
##### Windows
```
run-client.bat
```
##### Linux
```
run-client.sh
```