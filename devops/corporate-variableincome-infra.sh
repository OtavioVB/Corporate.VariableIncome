#!/bin/bash

set -e

echo "Criando Banco de Dados..."
docker compose -f "corporate-variableincome-database.yml" up -d

echo "Criando Apache Kafka..."
docker compose -f "corporate-variableincome-messenger.yml" up -d

echo "Criando Observabilidade..."
docker compose -f "corporate-variableincome-observability.yml" up -d
