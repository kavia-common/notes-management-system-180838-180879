# Notes API (In-Memory)

A minimal .NET 8 Web API for CRUD operations on notes. Uses an in-memory repository (singleton) suitable for local development and demos.

Base URL:
- http://localhost:3001

Docs:
- Swagger UI: http://localhost:3001/docs
- OpenAPI JSON: http://localhost:3001/openapi.json

Health:
- GET /

Endpoints:
- GET /api/notes -> 200 OK, list of notes
- GET /api/notes/{id} -> 200 OK or 404
- POST /api/notes -> 201 Created or 400
- PUT /api/notes/{id} -> 200 OK, 400, or 404
- DELETE /api/notes/{id} -> 204 No Content or 404

Model:
- id (GUID)
- title (required, <=256)
- content (optional, <=5000)
- createdAt (UTC)
- updatedAt (UTC)

Notes:
- CORS is permissive for local development.
- Storage is in-memory; data resets on service restart.
