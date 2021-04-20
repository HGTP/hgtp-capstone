'use strict';

// Copyright 2021 HGTP Capstone Team at the University of Utah: Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

const roleGroup = {
  admin: 'Gestr Admins',
};

/**
 * Compares the user's groups to the required role and determines if the user is
 * authorized to proceed. If not, an appropriate response is returned.
 * 
 * The purpose of this function is solely as a utility.
 * 
 * @param {Object} req 
 * @param {Object} res 
 */
const isAuthorized = (req, res) => {
  if (Object.keys(roleGroup).includes(req.params.role)) {
    const authorized = req.user.groups.includes(roleGroup[req.params.role]);
    res.status(200).send(authorized);
  } else {
    res.sendStatus(400);
  }
};

module.exports = (app, authGuard) => {
  app.get('/authorization/:role', authGuard(['Everyone']), isAuthorized);
};
