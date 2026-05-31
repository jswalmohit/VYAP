# Render Deployment Guide

## Environment Variables Required

When deploying to Render, set the following environment variable in your Render dashboard:

### DATABASE_CONNECTION_STRING
Your SQL Server connection string. Example:
```
Server=your-server.database.windows.net;Database=your-db;User Id=your-user;Password=your-password;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
```

**Important:** Ensure your SQL Server firewall rules allow connections from Render's IP range or make it publicly accessible.

## Steps

1. **Create a Web Service on Render**
   - Connect your GitHub repository
   - Select Docker as the runtime
   - Set the name and region

2. **Add Environment Variables in Render Dashboard**
   - Go to your service's Environment tab
   - Add `DATABASE_CONNECTION_STRING` with your SQL Server connection string
   - Optionally add `PORT` (defaults to 80)

3. **Deploy**
   - Render will automatically build and deploy from your `Dockerfile`

## Testing

Once deployed, check the status at:
```
https://your-service-name.onrender.com/api/StatusCheck
```

Response format:
```json
{
  "databaseConnected": true,
  "databaseError": null,
  "version": "1.0.0"
}
```

If `databaseConnected` is `false`, check the error message and verify:
- Connection string is correct
- SQL Server is publicly accessible or Render IP is whitelisted
- Database exists and credentials are valid

## Logs

View deployment and runtime logs in the Render dashboard under the service's "Logs" tab.
