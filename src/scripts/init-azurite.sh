#!/bin/sh
set -e

CONNECTION_STRING="${AZURE_STORAGE_CONNECTION_STRING:-DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://azurite:10000/devstoreaccount1;}"

for container in adega-images vinho-images user-images; do
  attempt=1
  until [ "$attempt" -gt 30 ]; do
    if az storage container create \
      --name "$container" \
      --connection-string "$CONNECTION_STRING" \
      --public-access blob \
      --only-show-errors >/dev/null 2>&1 \
      && az storage container set-permission \
      --name "$container" \
      --connection-string "$CONNECTION_STRING" \
      --public-access blob \
      --only-show-errors >/dev/null 2>&1; then
      break
    fi

    echo "A aguardar Azurite para o container $container (tentativa $attempt/30)..."
    attempt=$((attempt + 1))
    sleep 2
  done

  if [ "$attempt" -gt 30 ]; then
    echo "Falha ao configurar container $container no Azurite."
    exit 1
  fi

  echo "Container publico garantido: $container"
done

echo "Inicializacao do Azurite concluida."
