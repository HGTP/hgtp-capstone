export default {
  clientId: '', // Okta client id.
  issuer: '', // Okta issuer url.
  redirectUri: `${window.location.origin}/login/callback`,
  scopes: ['openid', 'profile', 'email'],
  pkce: true,
};
