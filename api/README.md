# Overview
This is the API for the HGTP capstone project.

# License
```
Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

# Requirements
- node.js
- local mariadb instance OR docker

# Setup
Copy `developer-env` to a file named `.env`.

Make sure to provide the correct database connection information in the `.env`. If using docker, you can leave it as is.

First off, run this command from the root folder to install dependencies:
- `npm run install`

## Without docker
If you have a local mariadb server up and running, and have provided the correct information in the `.env` file, then you can run the app using this command: `npm run start`.

If you want the api to be running with hot-reload enabled, use this command instead: `npm run dev:local`.

## With docker
Use the following commands if you want to use the docker-compose option instead:
- `docker-compose build`
- `docker-compose up`

For docker to work, the directory has to be accessible to docker. If you're using Docker Desktop, the folder can be added 
in the dashboard settings under `resource -> file sharing`.

# Running tests
Use the command `npm run test:unit` to run our tests.

# Sequelize (ORM)

To add a new model, use the following command:
`npx sequelize-cli model:generate --name SomeModelName --attributes somethingA:string,somethingB:string`
