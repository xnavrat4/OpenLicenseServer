# OpenLicenseServer

This project (more precisely solution) is consisting of three projects. 

 - Open License Managementent server 
 - Open License Server
 - SetupDbApp
 
 ## Setup
 
 Docker images can be obtained with dotnet publish command
 
 `cd /path/to/api/`
 
`dotnet publish --os linux --arch x64 /t:PublishContainer -c Release`

docker-compose.yml script is included in the repository for launching the system by running 

`cd /path/to/docker-compose.yml`

`docker-compose up`

Be aware, that a private key of the license  server is inluded in the docker-compose.yml. If the solution is to be deployed, the server's private should be changed and passed through user secrets. Also provide a valid cerftificate to enable HTTPS.


But imidiatelly after the DB is started, it has to be seeded by :

`docker run -e "OLSConnectionString=substituteYourConnectionString" --net=host xnavrat4/setupdb:1.0.0`



