# What is it?

Little program to simulate and capture TLS protected traffic to Azure Serivce Bus.

An illustration for https://stackoverflow.com/questions/75601341/does-servicebusreceiver-use-a-pull-or-a-push-communication-model-when-talking-to/75609529?noredirect=1#comment133406806_75609529.

# How to view packet capture?

1. Open `capture.pcapng` in WireShark
2. Go to Preferences > Protocols > TLS
3. Specify path to `master.sslkeylog` file
4. Use filter `amqp || tls`