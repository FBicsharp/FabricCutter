[![Test Build](https://github.com/FBicsharp/BracketsGym/actions/workflows/main.yml/badge.svg?branch=master)](https://github.com/FBicsharp/BracketsGym/actions/workflows/main.yml) 
# FabricCutter.UI 


# HOW TO RUN 

## REQUIREMENTS 
- docker
- docker compose version 3.8 or higher- 

## SETP UP
Download the repository, on project folder and run the docker compose command
```sh
git clone https://github.com/FBicsharp/BracketsGym.git
```
```sh
cd BracketsGym
```
```sh
docker compose up -d
```

the site will be available on http://localhost:33000 and the api on http://localhost:33500
```sh
http://localhost:33000
```
```sh
http://localhost:33500
```


# NOTE
> For the pdf generation i use a not free library that have a watermark on the pdf, if you want to remove it you need to buy a license.
> Unfortunatly this take a few minute to startup response at the first request, becouse i suppose that some license check is performed at the first request.

 
# ARCHITECTURE REVIEW 

In the case of many requests, it could be beneficial to introduce the use of Kubernetes to achieve a simpler and faster orchestration and deployment system. 
This would also provide an easier and quicker scalability system under a load balancer, enhancing both resilience and scalability and ensuring redundancy.

Alternatively, if you wish to maintain the current structure, you could consider adding a caching system to establish a queuing system for handling requests sequentially, 
potentially penalizing requests in case of peak loads. However, it's worth noting that this latter approach would still have limitations
The benefit of use docker is that we can run it everyware and we can have the same environment on every machine, so we can have the same result on every machine.
If we use the cloud providere such as azure or aws we can use the container service for have a better scalability and resilience, in this case we have a payment for a service that we can use only on cloud provider.
THe clould offer a lot of service that semplyfile interaction with various service such as database, storage, queue, etc. so it depends of the application that we need to develop.
if we need to develop a simple application that do not need a lot of interaction with other service we can use a simple docker container on a simple vm,
but if we need to develop a complex application that need to interact with other service we can use a cloud provider .
The other point o consider security, if we use a cloud provider we can use the security service that the cloud provider offer, 
but if we use a simple vm on premise we need to implement the security by ourself.



# TODO 

## BACKEND
- adding test runner on docker build
- add api documentation 

## FRONTEND
- adding unit test to UI
- adding test runner on docker build
- add tost notification or loading spinner 
