# URL Shortener

## User Stories

1. As User I want to put my URL and then get the shortener URL
2. As User I want to see where the shortened URL will lead
3. As User I want to see the acessible shorten URL if it exist (if the user URL is already in the database)

## HTTP API

- PUT(api/url BODY = {origin_url}) => save the record to database
- GET(api/url/{Id}) => redirect to the origin URL by ID
- GET(api/url/{ShortenedUrl}) => redirect to the origin URL by Shortened URL

## Models

**URL** :

- Id
- Origin URL
- Shortened URL
