'use strict';

// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

const OktaJwtVerifier = require('@okta/jwt-verifier');
const oktaJwtVerifier = new OktaJwtVerifier({
  issuer: process.env.ISSUER_URL,
});

/**
 * Returns a function which can be used to verify the authenticity
 * and authority of a request.
 * 
 * @param {Array} allowedRoles 
 * @returns {Function}
 */
const getAuthFunc = (allowedRoles) => {
  return async (req, res, next) => {
    try {
      const audience = 'api://default';
      const token = req.headers.authorization.replace('Bearer ', '');
      const jwt = await oktaJwtVerifier.verifyAccessToken(token, audience);
      if (!jwt.claims.groups.some((group) => allowedRoles.includes(group))) {
        res.sendStatus(403);
      } else {
        req.user = {
          email: jwt.claims.sub,
          groups: jwt.claims.groups,
        };
        next();
      }
    } catch (err) {
      console.log(err, 'Token failed validation');
      res.sendStatus(401)
    }
  };
};

const authorization = require('./authorization');
const featureRequests = require('./featureRequests');
const gesturePresets = require('./gesturePresets');

module.exports = (app) => {
  authorization(app, getAuthFunc);
  featureRequests(app, getAuthFunc);
  gesturePresets(app, getAuthFunc);
};
