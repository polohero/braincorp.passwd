# braincorp.passwd

Passwd as a Service
The idea of this challenge is to create a minimal HTTP service that exposes the user and group information on a UNIX-like system that is usually locked away in the UNIX /etc/passwd and /etc/groups files.

Solution Structure
The name of the service is BrainCorp.Passwd.WebService. The solution is structured with 9 projects total (including 3 Unit Test projects, and a Client). Common, Entities, DataAccess, Business, WebService, and 2 Testing projects.


Configurations
In the BrainCorp.Passwd.WebService project, you can modify the web.config for the following configurations.

userFilePath: The path to the user file. By default this will be set to users.txt in the local directory.
groupFilePath: The path to the group file. By default this will be set to groups.txt in the local directory.
fileLocation: An enumeration to let the service know where the above 2 files are. Acceptable values are Relative or Absolute. Relative means relative to the bin folder, absolute means absolute.
failBehavior: An enumeration to let the service know how it should handle corrupted or missing files. Acceptable values are ContinueWithOldData or FailWithCorruptedFile. The first time the service is executed it will load the files and cache them in memory. On each request it will check the last modified date of the file. If the file has not change it will used the cached value. This config tells the service to either fail, or continue with the cached data if there is a bad file. I suspect ContinueWithOldData is the best configuration as you wouldn't want a user to mess with a file to take down your whole system.
dataAccessSource: An enumeration to let the service know where the files reside. Acceptable values are FileOnDisk or FileInS3. This cloud version you are seeing is using FileInS3. In S3 the files will be https://s3.amazonaws.com/braincorppasswd/groups.txt and https://s3.amazonaws.com/braincorppasswd/user.txt
AWS*: These values are used to access the S3 buckets in AWS.
Example Call/Class Stack:
The entry points are: BrainCorp.Passwd.WebService, UsersController or GroupsController. Unity is used for dependancy injection.
For an example request: UserController would receive a request on public IHttpActionResult Search([FromUri] UserSearchRequest searchRequest)
UserSearchRequest is the class that holds all the search criteria. The UserSearchRequest is submitted to the PasswdProvider.
The PasswdProvider checks if there is a cache of the user information (className: Users). If there is a cache, and the file hasn't been updated it uses the cached object, otherwise it loads it on demand.
The PasswdProvider then uses the ToUserSearch method on the UserSearchRequest (using a Builder pattern), to build a UserSearch. That UserSearch is submitted to the Users object to search for users that fit that criteria.
The Users class is a collection of User objects. Internally it has organized the users in such a way to give O(1) search. To achive this, it uses the UserDataAccess to load all users from the file. Then, creates a hashtable (Dictionary) where the Key of the hash is the property, and the Value is a List[User]. Yes, this does mean that data is duplicated multiple times. I made the assumption that speed is important, so I've sacrificed memory for O(1) performance.
The List[User] is then returned to the caller.
Limitations/Considerations
If this project required a complete scalable solutions I would change a few things

Instead of reading the file locally and caching it in memory, I would move the "Users" object into a nosql store. So, I would essentially be moving all the Dictionary objects in Users/Groups into something like a DynamoDB that will shard.
To support the move to nosql, I would need a file watcher/Lambda function. When the file was changed, it would need to kick off a process to read all the information out of the file, and put it in the nosql store and any instance of the service would always be looking at the nosql database.
Doing it this way would introduce some latency (minutes likely, depending on the number of users/groups) and could result in dirty reads. After the file was updated, and before the file watcher completed uploading to the data store. Typically User operations in large businesses are cached in some form, so likely it would not be a problem to accept some time before changes are reflected live.
Runing the Service
Open the solution in Visual Studio. Then right click on BrainCorp.Passwd.WebService and press Debug->Start New Instance. The service will then open up a browser window on http://localhost:64099. In order to hit the endpoints you would need to add api to the url as well.

Example urls:
http://localhost:64099/api/users
http://localhost:64099/api/users/query?uid=1
http://localhost:64099/api/users/1
http://localhost:64099/api/users/1/groups
http://localhost:64099/api/groups
http://localhost:64099/api/groups/query?gid=1
http://localhost:64099/api/groups/1

Example cloud urls:
http://www.corywixom.com/api/users
http://www.corywixom.com/api/users/query?uid=1
http://www.corywixom.com/api/users/1
http://www.corywixom.com/api/users/1/groups
http://www.corywixom.com/api/groups
http://www.corywixom.com/api/groups/query?gid=1
http://www.corywixom.com/api/groups/1
