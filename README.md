# CalendarTodoList

## Requirements

- MS SQL Server

## Development

### Add connection string

In Visual Studio 2022, right-click on `CalendarTodoList.Server` then select `Manage User Secrets` and add a connection string as in the example.

```json
// secrets.json
{
  "ConnectionStrings": {
    "Default": "Server=localhost\\SQLEXPRESS;Database=CalendarTodoList;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Overview

### Normal
![](./.assets/home-page.png)
![](./.assets/login-page.png)
![](./.assets/register-page.png)
![](./.assets/calendar-page.png)
### New user
![](./.assets/new-user-register-page.png)
![](./.assets/new-user-calendar-page.png)

