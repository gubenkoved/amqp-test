#!/usr/bin/env bash

openssl genrsa -out sslsplit.key 2048
openssl req -new -x509 -days 365 -key sslsplit.key -out sslsplit.crt

echo "make sure now to add this CA certificate to trusted!"