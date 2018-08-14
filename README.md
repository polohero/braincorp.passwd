# braincorp.passwd

SOLUTION STRUCTURE:
The name of the service is BrainCorp.Passwd.WebService. The solution is structured with 9 projects total (including 3 Unit Test projects, and a Client). Common, Entities, DataAccess, Business, WebService, and 2 Testing projects.

CONFIGURATIONS:
In the BrainCorp.Passwd.WebService project, you can modify the web.config for the following configurations.

userFilePath: The path to the user file. By default this will be set to users.txt in the local directory.

groupFilePath: The path to the group file. By default this will be set to groups.txt in the local directory.

fileLocation: An enumeration to let the service know where the above 2 files are. Acceptable values are Relative or Absolute. Relative means relative to the bin folder, absolute means absolute.

failBehavior: An enumeration to let the service know how it should handle corrupted or missing files. Acceptable values are ContinueWithOldData or FailWithCorruptedFile. The first time the service is executed it will load the files and cache them in memory. On each request it will check the last modified date of the file. If the file has not change it will used the cached value. This config tells the service to either fail, or continue with the cached data if there is a bad file. I suspect ContinueWithOldData is the best configuration as you wouldn't want a user to mess with a file to take down your whole system.

dataAccessSource: An enumeration to let the service know where the files reside. Acceptable values are FileOnDisk or FileInS3. In S3 the files will be https://s3.amazonaws.com/braincorppasswd/groups.txt and https://s3.amazonaws.com/braincorppasswd/user.txt

AWS*: These values are used to access the S3 buckets in AWS.

EXAMPLE CALL/CALL STACK:
The entry points are: BrainCorp.Passwd.WebService, UsersController or GroupsController. Unity is used for dependancy injection.

For an example request: UserController would receive a request on public IHttpActionResult Search([FromUri] UserSearchRequest searchRequest)

UserSearchRequest is the class that holds all the search criteria. The UserSearchRequest is submitted to the PasswdProvider.
The PasswdProvider checks if there is a cache of the user information (className: Users). If there is a cache, and the file hasn't been updated it uses the cached object, otherwise it loads it on demand.

The PasswdProvider then uses the ToUserSearch method on the UserSearchRequest (using a Builder pattern), to build a UserSearch. That UserSearch is submitted to the Users object to search for users that fit that criteria.

The Users class is a collection of User objects. Internally it has organized the users in such a way to give O(1) search. To achive this, it uses the UserDataAccess to load all users from the file. Then, creates a hashtable (Dictionary) where the Key of the hash is the property, and the Value is a List[User]. Yes, this does mean that data is duplicated multiple times. I made the assumption that speed is important, so I've sacrificed memory for O(1) performance.
The List[User] is then returned to the caller.

LIMITATIONS/CONSIDERATIONS:
If this project required a complete scalable solutions I would change a few things

Instead of reading the file locally and caching it in memory, I would move the "Users" object into a nosql store. So, I would essentially be moving all the Dictionary objects in Users/Groups into something like a DynamoDB that will shard.

To support the move to nosql, I would need a file watcher/Lambda function. When the file was changed, it would need to kick off a process to read all the information out of the file, and put it in the nosql store and any instance of the service would always be looking at the nosql database.

Doing it this way would introduce some latency when updating the files (minutes likely, depending on the number of users/groups) and could result in dirty reads. After the file was updated, and before the file watcher completed uploading to the data store. Typically User operations in large businesses are cached in some form, so likely it would not be a problem to accept some time before changes are reflected live.

REQUIREMENTS:

Passwd as a Service:
The idea of this challenge is to create a minimal HTTP service that exposes the user and group information on
a UNIX-like system that is usually locked away in the UNIX /etc/passwd and /etc/groups files.
While this service is obviously a toy (and potentially a security nightmare), please treat it as you would a real
web service. That means write production quality code per your standards, including at least: Unit Tests, and
README documentation. Use any of the following languages and an idiomatic HTTP framework of your
choosing: Java/Kotlin/Scala, C#/F#, Python, Ruby, Go, JavaScript, or Rust. Please post your solution to a
public GitHub (or BitBucket or GitLab) repository and include instructions for running your service.
To aid testing and deployment, the paths to the passwd and groups file should be configurable, defaulting to
the standard system path. If the input files are absent or malformed, your service must indicate an error in a
manner you feel is appropriate.
This service is read-only but responses should reflect changes made to the underlying passwd and groups files
while the service is running. The service should provide the following methods:
GET /users
Return a list of all users on the system, as defined in the /etc/passwd file.
Example Response:
[
{“name”: “root”, “uid”: 0, “gid”: 0, “comment”: “root”, “home”: “/root”,
“shell”: “/bin/bash”},
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/home/dwoodlins”, “shell”: “/bin/false”}
]
GET
/users/query[?name=<nq>][&uid=<uq>][&gid=<gq>][&comment=<cq>][&home=<
hq>][&shell=<sq>]
Return a list of users matching all of the specified query fields. The bracket notation indicates that any of the
following query parameters may be supplied:
- name
- uid
- gid
- comment
- home
- shell
Only exact matches need to be supported.
Example Query: GET /users/query?shell=%2Fbin%2Ffalse
Example Response:
[
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/home/dwoodlins”, “shell”: “/bin/false”}
]
GET /users/<uid>
Return a single user with <uid>. Return 404 if <uid> is not found.
Example Response:
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/home/dwoodlins”, “shell”: “/bin/false”}
GET /users/<uid>/groups
Return all the groups for a given user.
Example Response:
[
{“name”: “docker”, “gid”: 1002, “members”: [“dwoodlins”]}
]
GET /groups
Return a list of all groups on the system, a defined by /etc/group.
Example Response:
[
{“name”: “_analyticsusers”, “gid”: 250, “members”:
[“_analyticsd’,”_networkd”,”_timed”]},
{“name”: “docker”, “gid”: 1002, “members”: []}
]
GET
/groups/query[?name=<nq>][&gid=<gq>][&member=<mq1>[&member=<mq2>][&.
..]]
Return a list of groups matching all of the specified query fields. The bracket notation indicates that any of the
following query parameters may be supplied:
- name
- gid
- member (repeated)
Any group containing all the specified members should be returned, i.e. when query members are a subset of
group members.
Example Query: GET /groups/query?member=_analyticsd&member=_networkd
Example Response:
[
{“name”: “_analyticsusers”, “gid”: 250, “members”:
[“_analyticsd’,”_networkd”,”_timed”]}
]
GET /groups/<gid>
Return a single group with <gid>. Return 404 if <gid> is not found.
Example Response:
{“name”: “docker”, “gid”: 1002, “members”: [“dwoodlins”]}
