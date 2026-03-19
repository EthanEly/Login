# Login

Welcome to the Login repository.

Here user's can either login, or if they don't have an account, they can register their details.

This consists of the User Details Manager service and the Authentication service.

The App for Login can be accessed through the User Details Manager App.

<strong>NOTE: You will need a .env file to get things running.</strong>

## Running the App

To install the dependancies for the app, run

```bash
npm run install:app
```

Once everything is installed, you can either run in dev mode, or in built mode.

### Dev mode

Dev mode includes hot reloading, which is advantageous when developing, but this feature does mean the app runs slower.

If this is your desired use case, run:

```bash
npm run dev:app
```

If you're not interested in making changes and seeing the app update on the fly, built mode is the one you want.

### Built mode

If just running the app is what you're after, no hot reloading needed, run:

```bash
npm run build:app
npm run start:app
```

## Running the API

To run the API, you can simply do a `npm run start:api`

This will build both micro-services, migrate their DBs and start them up.

# TL;DR

To simply get everything up at once:

```bash
npm run build
npm run start
```

## Once it's running...

Now that you've got all the services running, you can access the [landing page](http://localhost:3000).
