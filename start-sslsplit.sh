#!/usr/bin/env bash

# here I've used the IP which corresponds to the target Service Bus DNS name
# and I've edited the /etc/hosts so that the original DNS name resolves
# to the localhost so that TCP proxy can intercept it

sudo sslsplit -D -l connections.log -c sslsplit.crt -k sslsplit.key \
    -M master.sslkeylog \
    ssl 0.0.0.0 5671 52.168.133.227 5671