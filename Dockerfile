FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
EXPOSE 80

COPY *.sln .
COPY "/Brunsker.Bsnotas.SefazAdapter/Brunsker.Bsnotas.SefazAdapter.csproj" "/Brunsker.Bsnotas.SefazAdapter/"
COPY "/Brunsker.Bsnotas.WebApi/Brunsker.Bsnotas.WebApi.csproj" "/Brunsker.Bsnotas.WebApi/"
COPY "/Brunsker.Bsnotasapi.Application/Brunsker.Bsnotas.Application.csproj" "/Brunsker.Bsnotasapi.Application/"
COPY "/Brunsker.Bsnotasapi.Domain/Brunsker.Bsnotas.Domain.csproj" "/Brunsker.Bsnotasapi.Domain/"
COPY "/Brunsker.Bsnotasapi.OracleAdapter/Brunsker.Bsnotas.OracleAdapter.csproj" "/Brunsker.Bsnotasapi.OracleAdapter/"

RUN dotnet restore "/Brunsker.Bsnotas.WebApi/Brunsker.Bsnotas.WebApi.csproj"

COPY . ./
WORKDIR /app/Brunsker.Bsnotas.WebApi
RUN dotnet publish -c Release -o publish 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Brunsker.Bsnotas.WebApi/publish ./
ENTRYPOINT ["dotnet", "Brunsker.Bsnotas.WebApi.dll"]

RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update && apt-get -y install libxml2 libgdiplus libc6-dev

#Variaveis de ambiente Oracle
ENV TZ=America/Sao_Paulo



