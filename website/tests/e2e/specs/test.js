// https://docs.cypress.io/api/introduction/api.html

describe('My First Test', () => {
  it('Visits the app root url', () => {
    cy.visit('/');
    // TODO: This test has to be updated once we've added content, as this is the default stuff.
    cy.contains('h1', 'Welcome to Your Vue.js App');
  });
});
