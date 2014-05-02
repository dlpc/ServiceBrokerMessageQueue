#Service Broker Message Queue

Service Broker Message Queue(SBMQ) is a library and database script templates that implements a Message Queue over the SQL Server Message Broker Infrastructure.

Key goal of the SBMQ is to provide a drop in replacement for Microsoft Message Queues (MSMQ).

MSMQ Provides a solid underlying infrastructure for reliably sending and receiving messages.  However a number of shortcomings make SQL Server Service Broker an attractive alternative as a messaging platform.   

Key shortcomigs
 - Requires escalation to a distributed transaction when messaging operations involve database activities.
 - Requires additional configuration and infrastructure to establish a resiliant messaging environment.

SQL Server can address these shortcomings where database schema and service broker are on the same dataase and where the SQL Server instance has been configured for high availability.


##Service Broker Dialogs, Conversations and Services

Service Broker provides Dialogs, Conversations and Services to support the definition of a rich complex messaging architecture.  The full capabilities of service broker are not required to support a simple messaging send and receive messaging architecture


Security

Managability

##Service Broker Configuration for Message Qeueuing

98645