# Jwt Token Generation
Example of a real-world Jwt Authentication token generation implementation. The goal being to provide a centralized API to generate secure JWTs to be used by a UI to submit to a participating API for endpoint authorization. 

# Seed the Client table
```
Insert Into dbo.Client (ClientId, Name, Secret, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate)
Select
'3A0DABA5-760B-E711-8F39-782BCB46432F',
'AlfalfaStrange',
'7uoexWqG3WECvv3PL0RsK5dkfo17KnV4PpzQ+8QjFvg=',
1,
'2017-04-17 00:00:00.000',
null,
null
```
# Seed the Profile table
```
Insert Into dbo.Profile (ProfileTypeId, ProfileStatusTypeId, Username, Email, FirstName, LastName, Salt, PasswordHash, FailedLoginCount, CreatedDate, UpdatedBy, UpdatedDate)
Select
1,
1,
'Username',
'user@email.com',
'FirstName',
'LastName',
'CyXEtPGVj3wpZFwO4vMtbcy2nlZaZDa7HTcfP/sDITxyzjAuhzDeOFazNe9Xg2P/VrfxuXGlLl3dNCNNiMErD+cscRaeuuFVcdk5B5o9iF3/IA3ZCrKlfz0rmrGbiv04RGAiJF7PkCRZlSEmzMBaBjh3VEpN02b+FkP5y9u9rBw4ECVJ0b0d3qLuNL7d5tpS3cAXPvk3PdVE1uJWIWrn7oGNlIbOqaYDkufAWy1mxvQjzb8TKj4Gsyas3hEUDWKmHzZ9xnVmX+edCJvPu0fyCTPIETC3zfO51n9UdTSi/V0b2coX3ET7eUBpuyU26H+MHgiH3SPCWzRfw7SPWFFuLQ==',
'XeHlXtPMpDJRyh1CdAJL6lQDtpK6YIX1XekEjVqHw6o=',
0,
'2017-12-07 00:17:08.397',
null,
null
```
## Client Authentication and Authorization 
# Authenticate
Use a REST client and post the following
```
URL: http://localhost/jwt/api/token
Form: 
username = Username
password = Password123
client_id = 3A0DABA5-760B-E711-8F39-782BCB46432F
grant_type = password
```
# Example response
```
{
"access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJVc2VybmFtZSIsInByb2ZpbGVJZCI6IjIiLCJ1c2VybmFtZSI6IlVzZXJuYW1lIiwiZmlyc3ROYW1lIjoiRmlyc3ROYW1lIiwibGFzdE5hbWUiOiJMYXN0TmFtZSIsImVtYWlsIjoidXNlckBlbWFpbC5jb20iLCJyb2xlIjoiQWRtaW4iLCJpc3MiOiJBbGZhbGZhU3RyYW5nZSIsImF1ZCI6IjNBMERBQkE1LTc2MEItRTcxMS04RjM5LTc4MkJDQjQ2NDMyRiIsImV4cCI6MTUzNzE1MzY5MiwibmJmIjoxNTM3MTEwNDkyfQ.G8vtd80KVUOzhJb1-sLS_dEkq3PNFn7UlufkkzUpQbE",
"token_type": "bearer",
"expires_in": 43199
}
```
# Authorizing controller action

Use a REST client and GET the following
```
URL: http://localhost/jwt/api/v1/profiles
Add header:
Key: authorization
Value: Bearer [access_token value]
```

# Example response
```
{
"username": "Username",
"firstName": "FirstName",
"lastName": "LastName",
"fullName": "FirstName LastName",
"profileType": "Basic",
"profileStatusType": "Enabled"
}
```
# Elevated authorization required
```
URL: http://localhost/jwt/api/v1/profiles/secure
Add header:
Key: authorization
Value: Bearer [access_token value]

Response Status Code : 403
```
