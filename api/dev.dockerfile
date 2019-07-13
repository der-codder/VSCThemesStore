FROM mcr.microsoft.com/dotnet/core/sdk:2.2.301

WORKDIR /vsdbg

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    unzip \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh \
    | bash /dev/stdin -v latest -l /vsdbg

ENV DOTNET_USE_POLLING_FILE_WATCHER 1

WORKDIR /app/api

COPY VSCThemesStore.WebApi.csproj ./app/api/
RUN dotnet restore ./app/api/VSCThemesStore.WebApi.csproj

ENTRYPOINT dotnet watch run --urls=http://+:5021
