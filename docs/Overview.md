#Service Broker Message Queue

Service Broker Message Queue(SBMQ) is a library and database script templates that implements a Message Queue over the SQL Server Message Broker Infrastructure.

Key goal of the SBMQ is to provide a drop in replacement for Microsoft Message Queues (MSMQ).

MSMQ Provides a solid underlying infrastructure for reliably sending and receiving messages.  However a number of shortcomings make SQL Server Service Broker an attractive alternative as a messaging platform.   

Key shortcomings
 - Requires escalation to a distributed transaction when messaging operations involve database activities.
 - Requires additional configuration and infrastructure to establish a resiliant messaging environment.

SQL Server can address these shortcomings where database schema and service broker are on the same database and where the SQL Server instance has been configured for high availability.

##Service Broker Dialogs, Conversations and Services

SQL Server Service Broker provides Dialogs, Conversations and Services to support the definition of a rich & complex messaging architecture.  The full capabilities of service broker are not required to support a simple point to point messaging architecture.

##Point to Point Messaging with Service Broker
Point to point messaging enables a sender and a receiver to carry out a one way conversation where the sender puts a message on a queue and the receiver picks up the message from the queue.

Point to point messaging using infrastructure such as Websphere Message Queue, Microsoft Message Queue can be configured in either local or distributed scenarios.  The details of whether queue endpoints are local or distributed are largely hidden from the sender and receiver.

Sql Server Service Broker allows definitions of queues in both local and distributed scenarios using a wide range of configurations.  Using the nomeclature of Service Borker, to send a message from point *a* and receive at point *b* a client must *send* messages to a *initiator service* *a* with a *target service* specified.  The recipient then *receives* the message on the queue at point *b*

##Service Broker Services & Queue Convention
To define a basic point-to-point Service Broker messaging pattern within service broker, initiator and target services need to be created along with initiator and target queues.

To support a send to queue_name and receive from queue_name pattern a convention will be used to define the initiator and target service and queue names.

*Send Service and Queue*
- queue_name_initiator_service
- queue_name_initiator

*Receive Service and Queue*
- queue_name_service
- queue_name


##Queue Deployment
Service Broker Message Queues require a three pre-requisites for deployment
 - Service Broker to be enabled on the target database and;
 - A message_queue schema to be created in which the queus\services and management stored procs will reside and;
 - Management stored procs to be created on schema. 

Once the base Message Queue infrastructure has been deployed, queues can be created and deleted using the stored proceures.  Queue modification requires user with either dbo or ddladmin permissions.


##Queue Management

###Conversation Lifecycle

Start a Dialog, Send a message and End a Dialog so send a message.
Receive a message from the target queue

##Service Broker Configuration for Message Qeueuing
http://stackoverflow.com/a/1263999/395440 
IsEnqueue enabled

http://rusanu.com/2014/03/31/how-to-prevent-conversation-endpoint-leaks/
http://rusanu.com/2006/04/06/fire-and-forget-good-for-the-military-but-not-for-service-broker-conversations/

conversations SSB offers Exactly Once In Order delivery within a conversation so you can achieve FIFO

batch receives


Reusing Conversations
http://rusanu.com/2007/04/25/reusing-conversations/
http://rusanu.com/2007/05/03/recycling-conversations/ 


Message Cleanup
Ending the conversation

Security

Managability

