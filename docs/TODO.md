#Dev Environment
[ ] - Build to run performance test project
[X] - Check if MSDTC is started prior to running unit tests.
[X] - Start MSDTC prior to running unit tests
[X] - Create and drop database
[X] - Create Schema
[X] - Create stored procedures
[X] - Run UT's against dev database
 
#Send/Receive Code
[ ] - Reuse conversations/Handle Conversations correctly ?
[ ] - stored proc to finish dialog
[ ] - QueueManager
[ ] - Queue.Status
[ ] - Look at write code/stored procs for correctness and cleanliness
[X] - Read connection string from config file - not required.  User should pass in connection string.
[X] - Rationalise connection string creation method
[X] - Resolve connection pooling problem
[X] - QueueManager.QueueExists
[X] - Add code to allow sharing of dialogs
[X] - Add code to allow cleaning up of dialogs

#Production Readiness
[ ] - Set login on queue
[ ] - Remove login on queue
[ ] - Queue Statistics, Number Messages
[ ] - Purge/Clear Queue
[ ] - Remove Item from Queue
[ ] - Deployment
[ ] - Clean up conversations
[X] - remove_queue proc, delete inititor_service, target_service,target queue,initiator queue,contract, message type
[x] - remove queue test
[X] - GetQueues on queue manager



