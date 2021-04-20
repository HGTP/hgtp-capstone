'use strict';

// This is used for the package.json "postinstall" script which automatically runs on npm install
// or npm ci. This is so that when we deploy it to the instance in Azure, the database runs the
// newest migration.
//
// If we're running this in a pipeline we do not want to try to perform a database migration, we're
// avoiding this by removing the command using this script before installing dependencies in a pipeline.
// Note that this does not remove it from the repository, just from the file used during the pipeline.
const fs = require('fs');
const packageJson = require('../../../package.json');
delete packageJson.scripts.postinstall;
fs.writeFile(
  __dirname + '/../../../package.json',
  JSON.stringify(packageJson),
  () => console.log('Removed postinstall from package.json')
);
