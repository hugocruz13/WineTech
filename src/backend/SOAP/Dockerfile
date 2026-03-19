ARG MONO_IMAGE=mono:6.12

FROM ${MONO_IMAGE} AS build
WORKDIR /src

COPY . .

RUN nuget restore SOAP/packages.config -PackagesDirectory packages
RUN msbuild SOAP/SOAP.csproj /p:Configuration=Release

FROM ${MONO_IMAGE}
WORKDIR /app

RUN sed -i 's|deb.debian.org/debian|archive.debian.org/debian|g' /etc/apt/sources.list \
    && sed -i 's|deb.debian.org/debian-security|archive.debian.org/debian-security|g' /etc/apt/sources.list \
    && apt-get -o Acquire::Check-Valid-Until=false update \
    && DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends mono-xsp4 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /src/SOAP /app
COPY --from=build /src/packages /app/packages
COPY SOAP/run-soap.sh /run-soap.sh

RUN chmod +x /run-soap.sh

EXPOSE 8080

ENTRYPOINT ["/run-soap.sh"]
