# Overview
This is the Website for the HGTP capstone project.

# License
```
Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
```

# Requirements
- node.js
- api up and running if you want full functionality

# Setup
Run the following command from the root folder to install dependencies:
- `npm install`

Then run the command `npm run serve` to start a local instance of the website with hot reload enabled.

# Preparation for deployment
To prepare a production build of the website, run `npm run build`. This will create the production build in the `dist` folder.

## Deployment
The following command can be used to upload the production build to a storage account in Azure:
```
az storage blob upload-batch -s <absolute-path-to-dist-folder> -d '$web' --account-name <storage-account-name>
```

### Run your unit tests
```
npm run test:unit
```

### Lints and fixes files
```
npm run lint
```
