# UnikBolig
### This project was made as part of formal education at UCL and Unik.

## How to run with docker
### edit the .dockerfile and insert the project name (current name is "API")
```console
$ docker build --tag nameofproject .
$ docker run --publish internal-port:external-port --detach --name name_for_docker tag
```

### use -i for interactive console