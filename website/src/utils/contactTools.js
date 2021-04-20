// Copyright 2021 HGTP Capstone Team at the University of Utah:
// Emma Pinegar, Harrison Quick, Misha Griego, Jacob B. Norgaard
// Licensed under the MIT license. Read the project readme for details.

export const getName = () => {
  return 'John Doe';
};

export const getUsername = () => {
  return 'COOL_USER123';
};

export const getModals = () => {
  var modal = [];
  modal.push({
    modalTitle: 'Add Contact',
    inputOne: '',
    inputTwo: '',
    inputOneState: null,
    inputTwoState: null,
    inputOneLabel: 'Name',
    inputTwoLabel: 'Number',
  });
  return modal;
};
export const getContacts = () => {
  var contact = [];
  contact.push({
    name: 'Emma',
    number: '123-123-123',
  });
  contact.push({
    name: 'Harrison',
    number: '123-123-123',
  });
  contact.push({
    name: 'Jacob',
    number: '123-123-123',
  });
  contact.push({
    name: 'Misha',
    number: '123-123-123',
  });
  return contact;
};
