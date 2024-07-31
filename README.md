# Fitness Tracker API

This is a RESTful API for managing workouts and exercises. It allows you to perform CRUD operations on workouts and exercises using an in-memory database.

## Endpoints

### Workouts

- GET `/api/workouts` - Get all workouts
- GET `/api/workouts/{id}` - Get a specific workout by ID
- POST `/api/workouts` - Create a new workout
- PUT `/api/workouts/{id}` - Update an existing workout
- DELETE `/api/workouts/{id}` - Delete a workout

### Exercises

- GET `/api/exercises` - Get all exercises
- GET `/api/exercises/{id}` - Get a specific exercise by ID
- POST `/api/exercises` - Create a new exercise
- PUT `/api/exercises/{id}` - Update an existing exercise
- DELETE `/api/exercises/{id}` - Delete an exercise

## Running the API

1. Clone the repository.
2. Navigate to the project directory.
3. Run `dotnet restore` to install dependencies.
4. Run `dotnet run` to start the application.

## Examples

### Get all workouts

```sh
curl -X GET "https://localhost:5001/api/workouts"
