'use strict';

// This is where custom errors can be declared for error handling.

module.exports = (err, req, res, next) => {
  res.sendStatus(500);
  next();
};
